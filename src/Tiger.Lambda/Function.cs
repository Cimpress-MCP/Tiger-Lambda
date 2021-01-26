// <copyright file="Function.cs" company="Cimpress, Inc.">
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
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Microsoft.Extensions.Hosting.HostDefaults;

namespace Tiger.Lambda
{
    /// <summary>The base class and entry point of AWS Lambda Functions.</summary>
    public abstract class Function
    {
        IHost? _host;

        /// <summary>Gets the cancellation lead time.</summary>
        internal static TimeSpan CancellationLeadTime { get; } = TimeSpan.FromMilliseconds(500);

        /// <summary>Gets the application host.</summary>
        internal IHost Host => LazyInitializer.EnsureInitialized(ref _host, InitializeHost);

        /// <summary>Creates the host builder for the application.</summary>
        /// <returns>A host builder.</returns>
        public virtual IHostBuilder CreateHostBuilder() => LambdaHost.CreateDefaultBuilder();

        /// <summary>Configures the host builder for the application.</summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <returns>The modified host builder.</returns>
        public virtual IHostBuilder ConfigureHostBuilder(IHostBuilder hostBuilder) => hostBuilder;

        /// <summary>Adds services to the application dependency injection container.</summary>
        /// <param name="context">The context for building the application host.</param>
        /// <param name="services">The application's collection of services.</param>
        public abstract void ConfigureServices(
            HostBuilderContext context,
            IServiceCollection services);

        IHost InitializeHost()
        {
            var hostBuilder = CreateHostBuilder()
                .ConfigureHostConfiguration(config => config.AddInMemoryCollection(new[]
                {
                    // todo(cosborn) Is it really impossible to set a single configuration key directly???
                    KeyValuePair.Create(ApplicationKey, GetType().Assembly.GetName().Name),
                }));
            return ConfigureHostBuilder(hostBuilder)
                .ConfigureServices(ConfigureServices)
                .Build();
        }
    }
}
