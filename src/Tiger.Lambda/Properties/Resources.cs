// <copyright file="Resources.cs" company="Cimpress, Inc.">
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

using System.ComponentModel;
using System.Resources;
using System.Threading;
using static System.ComponentModel.EditorBrowsableState;

namespace Tiger.Lambda
{
    /// <summary>A strongly typed resource class for looking up localized strings, etc.</summary>
    static class Resources
    {
        static ResourceManager? s_resourceManager;
        static object s_resourceManagerLock = new();

        /// <summary>Gets the cached <see cref="ResourceManager"/> instance used by this class.</summary>
        [EditorBrowsable(Advanced)]
        public static ResourceManager ResourceManager => LazyInitializer.EnsureInitialized(
            ref s_resourceManager,
            ref s_resourceManagerLock,
            () => new ResourceManager("Tiger.Lambda.Resources", typeof(Resources).Assembly));

        /// <summary>Gets a message indicating a handler misconfiguration.</summary>
        public static string HandlerIsMisconfigured => ResourceManager.GetString(nameof(HandlerIsMisconfigured), null)!;

        /// <summary>Gets a message indicating an unhandled exception.</summary>
        public static string UnhandledException => ResourceManager.GetString(nameof(UnhandledException), null)!;

        /// <summary>Gets a message indicating that an execution handling has begun.</summary>
        public static string Handling => ResourceManager.GetString(nameof(Handling), null)!;

        /// <summary>Gets a message indicating that an execution is nearly out of time.</summary>
        public static string NearlyOutOfTime => ResourceManager.GetString(nameof(NearlyOutOfTime), null)!;

        /// <summary>Gets a message indicating that an execution has been canceled.</summary>
        public static string Canceled => ResourceManager.GetString(nameof(Canceled), null)!;
    }
}
