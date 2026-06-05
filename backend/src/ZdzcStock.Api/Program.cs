using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using ZdzcStock.Application.Services;
using ZdzcStock.Application.Validators;
using ZdzcStock.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();

builder
    .Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();

var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:300";

builder.Services.AddCors(optons =>
{
    optons.AddPolicy(
        "FrontendPolicy",
        policy =>
        {
            policy.WithOrigins(allowedOrigin).AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(
        "api",
        limiter =>
        {
            limiter.PermitLimit = 100;
            limiter.Window = TimeSpan.FromMinutes(1);
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(
        (doc, ctx, ct) =>
        {
            doc.Info = new()
            {
                Title = "Zdzc Stock API",
                Version = "v1",
                Description = "Api de gestão de catalogo de produtos e categorias",
            };
            return Task.CompletedTask;
        }
    );
});

builder.Services.AddExceptionHandler<ZdzcStock.Api.Middleware.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ZdzcStock API").WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseCors("FrontendPolicy");
app.UseRateLimiter();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
