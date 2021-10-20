// <copyright file="Function.cs" company="Cimpress, Inc.">
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

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Host = Microsoft.Extensions.Hosting.Host;

namespace Tiger.Lambda
{
    /// <summary>The base class and entry point of AWS Lambda Functions.</summary>
    public abstract class Function
    {
        /// <summary>Initializes a new instance of the <see cref="Function"/> class.</summary>
        internal Function()
        {
            ServiceProvider = InitializeServiceProvider();
        }

        /// <summary>Gets the cancellation lead time.</summary>
        internal static TimeSpan CancellationLeadTime { get; } = TimeSpan.FromMilliseconds(500);

        /// <summary>Gets the application service provider.</summary>
        internal IServiceProvider ServiceProvider { get; }

        /// <summary>Creates the host builder for the application.</summary>
        /// <returns>A host builder.</returns>
        public virtual IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder();

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

        IServiceProvider InitializeServiceProvider() =>
            ConfigureHostBuilder(CreateHostBuilder()).ConfigureServices(ConfigureServices).Build().Services;
    }
}
