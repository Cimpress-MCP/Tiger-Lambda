﻿// <copyright file="SecretsOptions.cs" company="Cimpress, Inc.">
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

namespace Tiger.Lambda
{
    /// <summary>
    /// Represents the declarative configuration options for AWS Secrets Manager configuration.
    /// </summary>
    public sealed class SecretsOptions
    {
        /// <summary>Gets or sets the base of the unique identifier of secrets.</summary>
        /// <remarks><para>
        /// This will be used as-is to retrieve secrets applicable
        /// to this application in all environments, and combined
        /// with the name of the current environment to retrieve
        /// secrets applicable to this application in the current
        /// environment.
        /// </para></remarks>
        public string BaseId { get; set; }
    }
}
