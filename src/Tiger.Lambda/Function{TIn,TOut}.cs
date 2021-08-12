// <copyright file="Function{TIn,TOut}.cs" company="Cimpress, Inc.">
//   Copyright 2020 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License") –
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Tiger.Lambda
{
    /// <summary>
    /// The base class and entry point of AWS Lambda Functions
    /// which return a value.
    /// </summary>
    /// <typeparam name="TIn">The type of the input to the Function.</typeparam>
    /// <typeparam name="TOut">The type of the output from the Function.</typeparam>
    public abstract class Function<TIn, TOut>
        : Function
    {
        /// <summary>Handles Lambda Function invocations.</summary>
        /// <param name="input">The input to the Function.</param>
        /// <param name="context">The context of this execution of the Function.</param>
        /// <returns>
        /// A task which, when resolved, results in the output from the Function.
        /// </returns>
        /// <exception cref="InvalidOperationException">The handler is misconfigured.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        public async Task<TOut> HandleAsync(TIn input, ILambdaContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            using var scope = Host.Services.CreateScope();

            var logger = scope.ServiceProvider.GetLogger(GetType());
            using var handlingScope = logger?.Handling(context);

            using var cts = new CancellationTokenSource(context.RemainingTime - CancellationLeadTime);
            using var warningRegistration = cts.Token.RegisterWarning(logger);

            try
            {
                return await HandleCoreAsync(
                    input,
                    context,
                    scope.ServiceProvider,
                    cts.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException tce) when (tce.CancellationToken == cts.Token)
            { // note(cosborn) Other timeouts can go into the catch-all handler.
                _ = warningRegistration.Unregister();
                logger?.Canceled(tce);
                throw;
            }
            catch (Exception e)
            {
                // note(cosborn) Log a nice message if we can.
                logger?.UnhandledException(GetType(), e);
                throw;
            }
        }

        /// <summary>The inner handler for Lambda Function invocations.</summary>
        /// <param name="input">The input to the Function.</param>
        /// <param name="context">The context of this execution of the Function.</param>
        /// <param name="serviceProvider">The application's provider of functional services.</param>
        /// <param name="cancellationToken">A token to watch for operation cancellation.</param>
        /// <returns>
        /// A task which, when resolved, results in the output from the Function.
        /// </returns>
        /// <exception cref="InvalidOperationException">The handler is misconfigured.</exception>
        [DebuggerHidden]
        internal virtual Task<TOut> HandleCoreAsync(
            TIn input,
            ILambdaContext context,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetHandler<TIn, TOut>();
            return handler.HandleAsync(input, context, cancellationToken);
        }
    }
}
