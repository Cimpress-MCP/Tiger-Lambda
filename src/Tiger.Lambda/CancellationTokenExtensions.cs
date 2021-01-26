// <copyright file="CancellationTokenExtensions.cs" company="Cimpress, Inc.">
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

using System.Threading;
using Microsoft.Extensions.Logging;

namespace Tiger.Lambda
{
    /// <summary>Extensions to the funcitonality of the <see cref="CancellationToken"/> struct.</summary>
    static class CancellationTokenExtensions
    {
        /// <summary>Registers the nearly-out-of-time warning.</summary>
        /// <param name="cancellationToken">The cancellation token on which to register the warning.</param>
        /// <param name="logger">The logger which will log the warning.</param>
        /// <returns>The registration token with which to cancel the warning.</returns>
        public static CancellationTokenRegistration RegisterWarning(
            this CancellationToken cancellationToken,
            ILogger? logger) => cancellationToken.Register(
                l => ((ILogger?)l)?.NearlyOutOfTime(),
                logger,
                useSynchronizationContext: false);
    }
}
