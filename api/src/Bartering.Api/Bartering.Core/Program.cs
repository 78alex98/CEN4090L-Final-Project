using Bartering.Core.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.ConfigureAuth(builder.Configuration)
    .AddDatabaseContexts(builder.Configuration)
    .AddApplicationServices();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Enable CORS to allow requests
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Bartering API Reference").WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
        options.Servers = []; // This is needed to ensure Scalar uses the correct connection when running in a container.
        options.DefaultOpenAllTags = false;
        options.HideModels = true;
        options.TagSorter = TagSorter.Alpha;
    });

    app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin"); //cors policy

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

// This is needed for integration testing to work.
// Access modifiers in the test configuration will conflict otherwise since Program is normally internal
//  and the testing framework requires tests to be public.
public partial class Program { }
