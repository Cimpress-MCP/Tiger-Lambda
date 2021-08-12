// <copyright file="IHandler{TIn,TOut}.cs" company="Cimpress, Inc.">
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

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace Tiger.Lambda
{
    /// <summary>
    /// Handles AWS Lambda Function invocations which return a value.
    /// </summary>
    /// <typeparam name="TIn">The type of the input to the Function.</typeparam>
    /// <typeparam name="TOut">The type of the output from the Function.</typeparam>
    public interface IHandler<TIn, TOut>
    {
        /// <summary>
        /// Handles an AWS Lambda Function invocation which returns a value.
        /// </summary>
        /// <param name="input">The input to the Function.</param>
        /// <param name="context">The context of this execution of the Function.</param>
        /// <param name="cancellationToken">A token to wach for operation cancellation.</param>
        /// <returns>
        /// A task which, when resolved, results in the output from the Function.
        /// </returns>
        Task<TOut> HandleAsync(
            TIn input,
            ILambdaContext context,
            CancellationToken cancellationToken = default);
    }
}
