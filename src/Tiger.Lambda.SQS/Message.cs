// <copyright file="Message.cs" company="Cimpress, Inc.">
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

/// <summary>A message from AWS Simple Queue Service.</summary>
public static class Message
{
    /// <summary>Creates a typed SQS message.</summary>
    /// <typeparam name="TBody">The type of <paramref name="body"/>, as deserialized from JSON.</typeparam>
    /// <param name="id">The unique identifier of the message.</param>
    /// <param name="body">The value of the message body, as deserialized from JSON.</param>
    /// <returns>A typed SQS message.</returns>
    public static Message<TBody> Create<TBody>(string id, TBody body) => new(id, body);
}

/// <summary>A message from AWS Simple Queue Service.</summary>
/// <typeparam name="TBody">The type of the message body, as deserialized from JSON.</typeparam>
/// <param name="Id">The unique identifier of the message.</param>
/// <param name="Body">The value of the message body, as deserialized from JSON.</param>
public record class Message<TBody>(string Id, TBody Body);
