// <copyright file="ServiceProviderExtensions.cs" company="Cimpress, Inc.">
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
using Microsoft.Extensions.Logging;
using static Tiger.Lambda.Resources;

namespace Tiger.Lambda
{
    /// <summary>Extensions to the functionality of the <see cref="IServiceProvider"/> interface.</summary>
    static class ServiceProviderExtensions
    {
        /// <summary>Gets the handler for the provided type.</summary>
        /// <typeparam name="TIn">The input type for which to construct a handler.</typeparam>
        /// <param name="serviceProvider">The service provider from which to construct a handler.</param>
        /// <returns>A handler for <typeparamref name="TIn"/>.</returns>
        public static IHandler<TIn> GetHandler<TIn>(this IServiceProvider serviceProvider)
        {
            try
            {
                return serviceProvider.GetRequiredService<IHandler<TIn>>();
            }
            catch (InvalidOperationException ioe)
            {
                // note(cosborn) Let's make the error message nicer.
                throw new InvalidOperationException(HandlerIsMisconfigured, ioe);
            }
        }

        /// <summary>Gets the handler for the provided types.</summary>
        /// <typeparam name="TIn">The input type for which to construct a handler.</typeparam>
        /// <typeparam name="TOut">The output type for which to construct a handler.</typeparam>
        /// <param name="serviceProvider">The service provider from which to construct a handler.</param>
        /// <returns>A handler for <typeparamref name="TIn"/> and <typeparamref name="TOut"/>.</returns>
        public static IHandler<TIn, TOut> GetHandler<TIn, TOut>(this IServiceProvider serviceProvider)
        {
            try
            {
                return serviceProvider.GetRequiredService<IHandler<TIn, TOut>>();
            }
            catch (InvalidOperationException ioe)
            {
                // note(cosborn) Let's make the error message nicer.
                throw new InvalidOperationException(HandlerIsMisconfigured, ioe);
            }
        }

        /// <summary>Gets a logger for the provided type.</summary>
        /// <param name="serviceProvider">The service provider from which to construct a logger.</param>
        /// <param name="type">The type for which to construct a logger.</param>
        /// <returns>A logger, or <see langword="null"/>.</returns>
        public static ILogger? GetLogger(this IServiceProvider serviceProvider, Type type) =>
            serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(type);
    }
}
