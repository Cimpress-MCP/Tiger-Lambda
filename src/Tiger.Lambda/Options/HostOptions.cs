// <copyright file="HostOptions.cs" company="Cimpress, Inc.">
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

namespace Tiger.Lambda;

/// <summary>The configuration options for the Lambda Host.</summary>
public sealed class HostOptions
    : IOptionsSnapshot<HostOptions>
{
    /// <summary>The default name of the configuration section.</summary>
    public const string Host = nameof(Host);

    /// <summary>Gets or sets the cancellation lead time.</summary>
    public TimeSpan CancellationLeadTime { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <inheritdoc/>
    HostOptions IOptions<HostOptions>.Value => this;

    /// <inheritdoc/>
    HostOptions IOptionsSnapshot<HostOptions>.Get(string name) => this;
}
