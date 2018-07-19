// <copyright file="Function{TIn,TOut}.cs" company="Cimpress, Inc.">
//   Copyright 2017 Cimpress, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
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
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using static Tiger.Lambda.Properties.Resources;

namespace Tiger.Lambda.StepFunctions
{
    /// <summary>
    /// The base class and entry point of AWS Lambda Functions
    /// which are used as part of a State Machine.
    /// </summary>
    /// <typeparam name="TIn">The type of the input to the Function.</typeparam>
    /// <typeparam name="TOut">The type of the output from the Function.</typeparam>
    /// <remarks><para>
    /// In AWS Step Functions, exceptions are used as part of state routing. Also in
    /// AWS Step Functions, Lambda Functions returning a <see cref="Task{TResult}"/>
    /// seem not to be `awaited`. This is somewhat problematic for C#, as asynchronous
    /// methods that are not `await`ed only throw <seealso cref="AggregateException"/>,
    /// with a collection of exceptions inside. Rather than improperly call
    /// <seealso cref="Task{TResult}.Result"/> all over the place, we can use the
    /// <see cref="AsyncContext"/> to manage, well, the async context. This prevents
    /// many kinds of nastiness in the async-land and causes the exceptions to be thrown
    /// correctly for our purposes.
    /// </para></remarks>
    public abstract class Function<TIn, TOut>
        : Function
    {
        /// <summary>Handles Lambda Function invocations.</summary>
        /// <param name="input">The input to the Function.</param>
        /// <param name="context">The context of this execution of the Function.</param>
        /// <returns>The output from the Function.</returns>
        /// <exception cref="InvalidOperationException">The handler is misconfigured.</exception>
        public TOut Handle([CanBeNull] TIn input, [NotNull] ILambdaContext context)
        {
            using (var scope = Host.Services.CreateScope())
            {
                IHandler<TIn, TOut> handler;
                try
                {
                    handler = scope.ServiceProvider.GetRequiredService<IHandler<TIn, TOut>>();
                }
                catch (InvalidOperationException ioe)
                {
                    // note(cosborn) Let's make the error message nicer.
                    throw new InvalidOperationException(HandlerIsMisconfigured, ioe);
                }

                try
                {
                    return AsyncContext.Run(() => handler.HandleAsync(input, context));
                }
                catch (Exception e)
                {
                    // note(cosborn) Log a nice message if we can.
                    var logger = scope.ServiceProvider.GetService<ILogger<Lambda.Function<TIn, TOut>>>();
                    using (logger?.BeginScope(context))
                    {
                        logger?.LogError(e, UnhandledException, GetType());
                    }

                    throw;
                }
            }
        }
    }
}
