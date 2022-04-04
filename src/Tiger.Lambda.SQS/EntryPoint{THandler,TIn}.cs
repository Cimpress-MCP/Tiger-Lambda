// <copyright file="EntryPoint{THandler,TIn}.cs" company="Cimpress, Inc.">
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

using Microsoft.Extensions.Hosting;

namespace Tiger.Lambda.Sqs;

/// <summary>The base class and entry point of AWS Lambda Functions which accept SQS events and return batch results.</summary>
/// <typeparam name="THandler">The type of the handler for the Function.</typeparam>
/// <typeparam name="TIn">The type of the records of the input to the Function.</typeparam>
public abstract class EntryPoint<THandler, TIn>
    : EntryPoint
    where THandler : class, IHandler<TIn>
{
    /// <summary>Handles Lambda Function invocations.</summary>
    /// <param name="input">The input to the Function.</param>
    /// <param name="context">The context of this execution of the Function.</param>
    /// <returns>A task which, when resolved, results in the output from the Function.</returns>
    /// <exception cref="InvalidOperationException">The handler is misconfigured.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
    public async Task<SQSBatchResponse> HandleAsync(SQSEvent input, ILambdaContext context)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(context);

        var scope = ServiceProvider.CreateAsyncScope();
        await using var scopeLifetime = scope.ConfigureAwait(false);

        var logger = scope.ServiceProvider.GetLogger(GetType());
        using var handlingScope = logger?.Handling(context);

        var hostOpts = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<HostOptions>>();
        using var cts = new CancellationTokenSource(context.RemainingTime - hostOpts.Value.CancellationLeadTime);
        var warningRegistration = cts.Token.RegisterWarning(logger);
        await using var warningRegistrationLifetime = warningRegistration.ConfigureAwait(false);
        try
        {
            var jsonOpts = scope.ServiceProvider.GetService<IOptionsSnapshot<JsonSerializerOptions>>();
            var records = input
                .Records
                .Select(r => (r.MessageId, Body: JsonSerializer.Deserialize<TIn>(r.Body, jsonOpts?.Value)))
                .Where(p => p.Body is not null)
                .Select(p => Message.Create(p.MessageId, p.Body!));
            var handler = scope.ServiceProvider.GetHandler<TIn>();
            var failures = await handler
                .HandleAsync(records, context, cts.Token)
                .Select(id => new BatchItemFailure
                {
                    ItemIdentifier = id,
                })
                .ToListAsync(cts.Token)
                .ConfigureAwait(false);
            return new(failures);
        }
        catch (OperationCanceledException oce) when (oce.CancellationToken == cts.Token)
        { // note(cosborn) Other timeouts can go into the catch-all handler.
            _ = warningRegistration.Unregister();
            logger?.Canceled(oce);
            throw;
        }
        catch (Exception e)
        {
            // note(cosborn) Log a nice message if we can.
            logger?.UnhandledException(GetType(), e);
            throw;
        }
    }

    /// <inheritdoc/>
    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        base.ConfigureServices(context, services);
        _ = services.AddScoped<IHandler<TIn>, THandler>();
    }
}
