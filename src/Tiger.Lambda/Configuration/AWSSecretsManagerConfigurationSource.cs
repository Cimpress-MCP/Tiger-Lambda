// <copyright file="AWSSecretsManagerConfigurationSource.cs" company="Cimpress, Inc.">
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
using Amazon.SecretsManager;
using JetBrains.Annotations;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>A source of AWS Secrets Manager configuration key/values for an application.</summary>
    public sealed class AWSSecretsManagerConfigurationSource
        : IConfigurationSource
    {
        /// <summary>Initializes a new instance of the <see cref="AWSSecretsManagerConfigurationSource"/> class.</summary>
        /// <param name="secretsManagerClient">The client to use to retrieve secret values.</param>
        /// <param name="secretId">The unique identifer of the secret to use for configuration.</param>
        /// <exception cref="ArgumentNullException"><paramref name="secretsManagerClient"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="secretId"/> is <see langword="null"/>.</exception>
        public AWSSecretsManagerConfigurationSource(
            [NotNull] IAmazonSecretsManager secretsManagerClient,
            [NotNull] string secretId)
        {
            SecretsManagerClient = secretsManagerClient ?? throw new ArgumentNullException(nameof(secretsManagerClient));
            SecretId = secretId ?? throw new ArgumentNullException(nameof(secretId));
        }

        /// <summary>Gets the client to use to retrieve secret values.</summary>
        public IAmazonSecretsManager SecretsManagerClient { get; }

        /// <summary>Gets the unique identifer of the secret to use for configuration.</summary>
        public string SecretId { get; }

        /// <inheritdoc/>
        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new AWSSecretsManagerConfigurationProvider(this);
    }
}
