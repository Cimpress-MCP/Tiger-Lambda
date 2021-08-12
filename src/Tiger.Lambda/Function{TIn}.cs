// <copyright file="Function{TIn}.cs" company="Cimpress, Inc.">
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
    /// which perform an action.
    /// </summary>
    /// <typeparam name="TIn">The type of the input to the Function.</typeparam>
    public abstract class Function<TIn>
        : Function<TIn, object?>
    {
        /// <inheritdoc/>
        [DebuggerHidden]
        internal override async Task<object?> HandleCoreAsync(
            TIn input,
            ILambdaContext context,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetHandler<TIn>();
            await handler.HandleAsync(input, context, cancellationToken).ConfigureAwait(false);
            return default;
        }
    }
}
