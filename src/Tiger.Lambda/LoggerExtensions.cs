// <copyright file="LoggerExtensions.cs" company="Cimpress, Inc.">
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
using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using Tiger.Lambda.Properties;
using static Microsoft.Extensions.Logging.LogLevel;

namespace Tiger.Lambda
{
    /// <summary>Extensions to the functionality of the <see cref="ILogger"/> interface.</summary>
    static class LoggerExtensions
    {
        static readonly Func<ILogger, string, IDisposable> _handlingScope = LoggerMessage.DefineScope<string>(
            Resources.Handling);

        static readonly Action<ILogger, Type, Exception?> _unhandledException = LoggerMessage.Define<Type>(
            Error,
            new EventId(1, nameof(UnhandledException)),
            Resources.UnhandledException);

        static readonly Action<ILogger, Exception?> _canceled = LoggerMessage.Define(
            Error,
            new EventId(2, nameof(Canceled)),
            Resources.Canceled);

        static readonly Action<ILogger, Exception?> _nearlyOutOfTime = LoggerMessage.Define(
            Error,
            new EventId(3, nameof(NearlyOutOfTime)),
            Resources.NearlyOutOfTime);

        /// <summary>Creates a logging scope for handling a request.</summary>
        /// <param name="logger">An application logger.</param>
        /// <param name="context">The context of the Lambda Function.</param>
        /// <returns>A value which, when disposed, will end the logging scope.</returns>
        public static IDisposable Handling(this ILogger logger, ILambdaContext context) =>
            _handlingScope(logger, context.AwsRequestId);

        /// <summary>
        /// Writes an error log message corresponding to the event of handling's failure.
        /// </summary>
        /// <param name="logger">An application logger.</param>
        /// <param name="type">The type of the handler in which the exeption occured.</param>
        /// <param name="exception">The exception thrown as a result of handling's failure.</param>
        public static void UnhandledException(this ILogger logger, Type type, Exception exception) =>
            _unhandledException(logger, type, exception);

        /// <summary>
        /// Writes an error log message corresponding to the event of a Function's cancellation.
        /// </summary>
        /// <param name="logger">An application logger.</param>
        /// <param name="exception">An exception which was the result of timing out.</param>
        public static void Canceled(this ILogger logger, Exception? exception = null) =>
            _canceled(logger, exception);

        /// <summary>
        /// Writes an error log message corresponding to the event of a Function's imminent termination.
        /// </summary>
        /// <param name="logger">An application logger.</param>
        public static void NearlyOutOfTime(this ILogger logger) => _nearlyOutOfTime(logger, null);
    }
}
