// <copyright file="IHandler{TIn}.cs" company="Cimpress, Inc.">
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

namespace Tiger.Lambda.Sqs;

/// <summary>Handles AWS Lambda Function invocations which perform an action.</summary>
/// <typeparam name="TIn">The type of the input to the Function.</typeparam>
public interface IHandler<TIn>
{
    /// <summary>Handles an AWS Lambda Function batched SQS invocation.</summary>
    /// <param name="input">The input to the Function.</param>
    /// <param name="context">The context of this execution of the Function.</param>
    /// <param name="cancellationToken">A token to wach for operation cancellation.</param>
    /// <returns>An asynchronous generator of the unique identifier of messages in the batch which failed.</returns>
    public IAsyncEnumerable<string> HandleAsync(IEnumerable<Message<TIn>> input, ILambdaContext context, CancellationToken cancellationToken = default);
}
