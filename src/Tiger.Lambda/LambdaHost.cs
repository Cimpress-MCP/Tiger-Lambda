// <copyright file="LambdaHost.cs" company="Cimpress, Inc.">
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
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Tiger.Lambda
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHostBuilder"/>
    /// with pre-configured defaults.
    /// </summary>
    public static class LambdaHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder() => WrapBuilder((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            var reloadOnChange = GetReloadConfigOnChangeValue(hostingContext);

            _ = config
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

            // todo(cosborn) User secrets are currently impossible due to a lack of HOME environment variable.
            _ = config.AddEnvironmentVariables();

            static bool GetReloadConfigOnChangeValue(HostBuilderContext hostingContext) => hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);
        });

        static IHostBuilder WrapBuilder(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate) => new HostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureHostConfiguration(config => config.AddEnvironmentVariables(prefix: "DOTNET_"))
            .ConfigureAppConfiguration(configureDelegate)
            .UseDefaultServiceProvider((context, options) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });
    }
}
