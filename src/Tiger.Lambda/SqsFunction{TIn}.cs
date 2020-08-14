// <copyright file="SqsFunction{TIn}.cs" company="Cimpress, Inc.">
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tiger.Lambda
{
    /// <summary>
    /// The base class and entry point of AWS Lambda Functions
    /// which accept SQS events and perform an action.
    /// </summary>
    /// <typeparam name="TIn">
    /// The type of the records of the input to the Function.
    /// </typeparam>
    public abstract class SqsFunction<TIn>
        : Function<SQSEvent>
    {
        /// <inheritdoc/>
        public override async Task HandleAsync([DisallowNull] SQSEvent input, ILambdaContext context)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input));}
            if (context is null) { throw new ArgumentNullException(nameof(context)); }

            using var scope = Host.Services.CreateScope();
            var handler = scope.GetHandler<IEnumerable<TIn>>();

            var logger = scope.GetLogger(GetType());
            using var @finally = logger?.Handling(context);
            try
            {
                var jsonOpts = scope.ServiceProvider.GetService<IOptions<JsonSerializerOptions>>();
                var records = input
                    .Records
                    .Select(r => r.Body)
                    .Select(b => JsonSerializer.Deserialize<TIn>(b, jsonOpts?.Value));
                await handler.HandleAsync(records, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // note(cosborn) Log a nice message if we can.
                logger?.UnhandledException(GetType(), e);
                ExceptionDispatchInfo.Capture(e).Throw();
                throw; // note(cosborn) Unreachable.
            }
        }
    }
}
