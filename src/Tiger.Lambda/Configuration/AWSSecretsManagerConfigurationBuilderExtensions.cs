// <copyright file="AWSSecretsManagerConfigurationBuilderExtensions.cs" company="Cimpress, Inc.">
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

using Amazon.SecretsManager;
using JetBrains.Annotations;
using Tiger.Lambda;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>Extends the functionality of <see cref="IConfigurationBuilder"/> for AWS Secrets Manager.</summary>
    public static class AWSSecretsManagerConfigurationBuilderExtensions
    {
        const string SectionName = "Secrets";

        /// <summary>Adds AWS Secrets Manager as a configuration source.</summary>
        /// <param name="builder">The configuration builder to which to add.</param>
        /// <param name="environmentName">The name of the environment in which the application is running.</param>
        /// <returns>The modified configuration builder.</returns>
        [NotNull]
        public static IConfigurationBuilder AddAWSSecretsManager(
            [NotNull] this IConfigurationBuilder builder,
            [NotNull] string environmentName)
        {
            /* because(cosborn)
             * I hate doing this, but:
             * 1. We want to call Build() once and reuse the IAmazonSecretsManager instance if we can. (And we can.)
             * 2. We want the IAmazonSecretsManager instance to have a _hope_ of being configured.
             * There must be something better??? (spoiler: there's not)
             */
            var configuration = builder.AddEnvironmentVariables().Build();
            var secretsManagerClient = configuration.GetAWSOptions().CreateServiceClient<IAmazonSecretsManager>();
            var secretsOpts = configuration.GetSection(SectionName).Get<SecretsOptions>();

            // note(cosborn) It's faster, due to how the JIT works, to do these separately. Yes, I benchmarked it.
            return builder
                .Add(new AWSSecretsManagerConfigurationSource(secretsManagerClient, secretsOpts.BaseId))
                .Add(new AWSSecretsManagerConfigurationSource(secretsManagerClient, $"{secretsOpts.BaseId}/{environmentName}"));
        }
    }
}
