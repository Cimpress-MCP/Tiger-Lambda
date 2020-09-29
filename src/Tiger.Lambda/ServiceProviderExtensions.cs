// <copyright file="ServiceProviderExtensions.cs" company="Cimpress, Inc.">
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Tiger.Lambda.Properties.Resources;

namespace Tiger.Lambda
{
    /// <summary>Extensions to the functionality of the <see cref="IServiceProvider"/> interface.</summary>
    static class ServiceProviderExtensions
    {
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

        public static ILogger? GetLogger(this IServiceProvider serviceProvider, Type type) =>
            serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(type);
    }
}
