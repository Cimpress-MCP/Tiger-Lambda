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

namespace Tiger.Lambda.Sqs;

/// <summary>Extensions to the functionality of the <see cref="IServiceProvider"/> interface.</summary>
static class ServiceProviderExtensions
{
    /// <summary>Gets the handler for the provided type.</summary>
    /// <typeparam name="TIn">The input type for which to construct a handler.</typeparam>
    /// <param name="serviceProvider">The service provider from which to construct a handler.</param>
    /// <returns>A handler for <typeparamref name="TIn"/>.</returns>
    /// <exception cref="InvalidOperationException">The handler could not be resolved due to misconfiguration.</exception>
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
}
