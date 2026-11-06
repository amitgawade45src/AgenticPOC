using BankService.Extensions;
using BankService.Logging;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;

// Bootstrap logger captures start-up failures before the host and configuration are available.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog replaces the default Microsoft.Extensions.Logging providers; ILogger<T> flows to Serilog.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithAssemblyName()
        .Enrich.WithCorrelationId()
        .Enrich.With(new TraceIdEnricher()));

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddControllers();

    // Register the Application + Infrastructure layers (composition root).
    builder.Services.AddBankServices(builder.Configuration);

    // MCP server (Streamable HTTP at /mcp) exposing the banking use cases as tools for AI agents.
    builder.Services.AddMcpServer()
        .WithHttpTransport()
        .WithToolsFromAssembly();

    // OpenAPI document generation for the REST API (served at /openapi/v1.json).
    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, _, _) =>
        {
            document.Info.Title = "Bank Service API";
            document.Info.Version = "v1";
            document.Info.Description =
                "CQRS-based banking API covering customer management, bank accounts and onboarding documents, " +
                "instant loans and home loans.";
            return Task.CompletedTask;
        });
    });

    builder.Services.AddOpenTelemetry()
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // OpenAPI JSON + interactive API reference UI (https://localhost:7208/scalar/v1).
        app.MapOpenApi();
        app.MapScalarApiReference(options => options.WithTitle("Bank Service API"));
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Single structured log line per HTTP request instead of the verbose default ASP.NET Core request logs.
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();
    app.MapRazorPages()
       .WithStaticAssets();
    app.MapControllers();
    app.MapMcp("/mcp");

    Log.Information("Bank Service starting up");
    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Bank Service terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
