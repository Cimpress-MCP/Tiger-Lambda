// <copyright file="Function.cs" company="Cimpress, Inc.">
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Microsoft.Extensions.Hosting.HostDefaults;

namespace Tiger.Lambda
{
    /// <summary>The base class and entry point of AWS Lambda Functions.</summary>
    public abstract class Function
    {
        /// <summary>Initializes a new instance of the <see cref="Function"/> class.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2214", Justification = "No way out, in a Lambda Function.")]
        protected Function()
        {
            var hostBuilder = CreateHostBuilder()
                .ConfigureHostConfiguration(config => config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    // todo(cosborn) Is it really impossible to set a configuration key directly???
                    [ApplicationKey] = GetType().GetTypeInfo().Assembly.GetName().Name
                }));
            Host = ConfigureHostBuilder(hostBuilder)
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        /// <summary>Gets the application host.</summary>
        internal IHost Host { get; }

        /// <summary>Creates the host builder for the application.</summary>
        /// <returns>A host builder.</returns>
        [NotNull]
        public virtual IHostBuilder CreateHostBuilder() => LambdaHost.CreateDefaultBuilder();

        /// <summary>Configures the host builder for the application.</summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <returns>The modified host builder.</returns>
        [NotNull]
        public virtual IHostBuilder ConfigureHostBuilder([NotNull] IHostBuilder hostBuilder) =>
            hostBuilder;

        /// <summary>Adds services to the application dependency injection container.</summary>
        /// <param name="context">The context for building the application host.</param>
        /// <param name="services">The application's collection of services.</param>
        public abstract void ConfigureServices(
            [NotNull] HostBuilderContext context,
            [NotNull, ItemNotNull] IServiceCollection services);
    }
}
