// <copyright file="HostBuilderExtensions.cs" company="Cimpress, Inc.">
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
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using static Microsoft.Extensions.DependencyInjection.ServiceDescriptor;

namespace Tiger.Lambda
{
    /// <summary>Extensions to the functionality of the <see cref="IHostBuilder"/> interface.</summary>
    static class HostBuilderExtensions
    {
        /// <summary>Configures the default service provider.</summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder UseDefaultServiceProvider(
            [NotNull] this IHostBuilder hostBuilder,
            [NotNull] Action<ServiceProviderOptions> configure)
        {
            if (hostBuilder is null) { throw new ArgumentNullException(nameof(hostBuilder)); }
            if (configure is null) { throw new ArgumentNullException(nameof(configure)); }

            return hostBuilder.UseDefaultServiceProvider((_, options) => configure(options));
        }

        /// <summary>Configures the default service provider.</summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder UseDefaultServiceProvider(
            [NotNull] this IHostBuilder hostBuilder,
            [NotNull] Action<HostBuilderContext, ServiceProviderOptions> configure)
        {
            if (hostBuilder is null) { throw new ArgumentNullException(nameof(hostBuilder)); }
            if (configure is null) { throw new ArgumentNullException(nameof(configure)); }

            return hostBuilder.ConfigureServices((context, services) =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                services.Replace(Singleton<IServiceProviderFactory<IServiceCollection>>(new DefaultServiceProviderFactory(options)));
            });
        }
    }
}
