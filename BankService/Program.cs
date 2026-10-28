using BankService.Extensions;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Register the Application + Infrastructure layers (composition root).
builder.Services.AddBankServices(builder.Configuration);

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

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

await app.RunAsync();
