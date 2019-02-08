// <copyright file="AWSSecretsManagerConfigurationProvider.cs" company="Cimpress, Inc.">
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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using static Microsoft.Extensions.Configuration.ConfigurationPath;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>Provides AWS Secrets Manager configuration key/values for an application.</summary>
    public sealed class AWSSecretsManagerConfigurationProvider
        : ConfigurationProvider
    {
        const string AlternativeKeyDelimiter = "__";

        readonly IAmazonSecretsManager _client;
        readonly string _secretId;

        /// <summary>Initializes a new instance of the <see cref="AWSSecretsManagerConfigurationProvider"/> class.</summary>
        /// <param name="configurationSource">The source of AWS Secrets Manager configuration.</param>
        /// <exception cref="ArgumentNullException"><paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public AWSSecretsManagerConfigurationProvider([NotNull] AWSSecretsManagerConfigurationSource configurationSource)
        {
            if (configurationSource is null) { throw new ArgumentNullException(nameof(configurationSource)); }

            _client = configurationSource.SecretsManagerClient;
            _secretId = configurationSource.SecretId;
        }

        /// <inheritdoc/>
        // because(cosborn) Configuration is purely a sync API, and we want good exceptions. Gross, gross, gross.
        public override void Load() => LoadCore().GetAwaiter().GetResult();

        [NotNull]
        async Task LoadCore()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = _secretId
            };
            var response = await GetSecretValueAsync(request).ConfigureAwait(false);
            if (response?.SecretString is null) { return; }

            Data = NormalizeData(JObject.Parse(response.SecretString));

            async ValueTask<GetSecretValueResponse> GetSecretValueAsync(GetSecretValueRequest req)
            {
                try
                {
                    return await _client.GetSecretValueAsync(req).ConfigureAwait(false);
                }
                catch (ResourceNotFoundException)
                {
                    // note(cosborn) I think we can't guarantee both an environment-specific secret and a generalized one.
                    return new GetSecretValueResponse();
                }
            }
        }

        [NotNull]
        IDictionary<string, string> NormalizeData([NotNull] JObject rawData)
        {
            /* note(cosborn)
             * "Normalize" the data? But it's already in proper JSON form. Why?
             *
             * The configuration providers that read JSON from files allow
             * configuration to be specified in two ways. Normalization means that:
             *
             * { "Compound": { "Key": "value" } }
             *
             * ...and:
             *
             * { "Compound:Key": "value" }
             *
             * ...are equivalent, and the latter is canonical. I do the same
             * operations here, as it's convenient to specify configuration as
             * key/value pairs in Secrets Manager. (It's still read back as
             * a JSON object, natch.) If raw JSON is entered directly, however,
             * we'll still do the right thing.
             */

            var data = new Dictionary<string, string>();
            /* because(cosborn)
             * This semi-functional, semi-imperative setup may look odd,
             * but it's much faster than the purely functional implementation
             * -- in which data is an immutable dictionary, passed around --
             * and it's faster than the purely imperative implementation
             * -- in which context is mutable and lives outside with data.
             * YES I BENCHMARKED IT
             */
            VisitObject(rawData, ImmutableArray<string>.Empty);
            return data;

            void VisitObject(JObject @object, ImmutableArray<string> context)
            {
                foreach (var property in @object.Properties())
                {
                    VisitToken(property.Value, context.Add(property.Name));
                }
            }

            void VisitArray(JArray array, ImmutableArray<string> context)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    /* note(cosborn)
                     * Remember, configuration considers arrays to be objects with "numeric" indices.
                     * That's why they merge how they do in AppSettings.
                     */
                    VisitToken(array[i], context.Add(i.ToString(CultureInfo.InvariantCulture)));
                }
            }

            void VisitToken(JToken token, ImmutableArray<string> context)
            {
                switch (token.Type)
                {
                    case JTokenType.Object:
                        VisitObject((JObject)token, context);
                        break;
                    case JTokenType.Array:
                        VisitArray((JArray)token, context);
                        break;
                    default:
                        // note(cosborn) ¯\_(ツ)_/¯
                        VisitValue((JValue)token, context);
                        break;
                }
            }

            void VisitValue(JValue value, ImmutableArray<string> context)
            {
                var key = NormalizeKey(Combine(context));

                // note(cosborn) If you create JSON with duplicate keys, you get what you get.
                data[key] = value.ToString(CultureInfo.InvariantCulture);

                // note(cosborn) Colons put into Secrets Manager keys can't always be extracted.
                string NormalizeKey(string k) => k.Replace(AlternativeKeyDelimiter, KeyDelimiter, StringComparison.Ordinal);
            }
        }
    }
}
