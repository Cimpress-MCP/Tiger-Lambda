// <copyright file="SqsFunction{TIn,TOut}.cs" company="Cimpress, Inc.">
//   Copyright 2021 Cimpress, Inc.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tiger.Lambda
{
    /// <summary>
    /// The base class and entry point of AWS Lambda Functions
    /// which accept SQS events and return a value.
    /// </summary>
    /// <typeparam name="TIn">
    /// The type of the records of the input to the Function.
    /// </typeparam>
    /// <typeparam name="TOut">The type of the output from the Function.</typeparam>
    public abstract class SqsFunction<TIn, TOut>
        : Function<SQSEvent, TOut>
    {
        /// <inheritdoc/>
        [DebuggerHidden]
        internal sealed override Task<TOut> HandleCoreAsync(
            SQSEvent input,
            ILambdaContext context,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var jsonOpts = serviceProvider.GetService<IOptionsSnapshot<JsonSerializerOptions>>();
            var records = input
                .Records
                .Select(r => r.Body)
                .Select(b => JsonSerializer.Deserialize<TIn>(b, jsonOpts?.Value));
            var handler = serviceProvider.GetHandler<IEnumerable<TIn>, TOut>();
            return handler.HandleAsync(records, context, cancellationToken);
        }
    }
}
