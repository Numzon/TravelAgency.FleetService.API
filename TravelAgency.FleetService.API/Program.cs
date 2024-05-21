using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using TravelAgency.FleetService.API.Configurations;
using TravelAgency.FleetService.API.Infrastructure.Extensions;
using TravelAgency.FleetService.API.Infrastructure.Persistance;
using TravelAgency.SharedLibrary.AWS;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.SharedLibrary.Vault;
using TravelAgency.SharedLibrary.Vault.Consts;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Information()
       .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day)
       .CreateLogger();

builder.Services.AddFastEndpoints()
    .SwaggerDocument(options =>
    {
        options.AutoTagPathSegmentIndex = 0;
    });

builder.Services.RegisterMapsterConfiguration();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

if (builder.Environment.IsProduction())
{
    var vaultBuilder = new VaultFacadeBuilder();

    var vaultFacade = vaultBuilder
                        .SetToken(Environment.GetEnvironmentVariable(VaultEnvironmentVariables.Token))
                        .SetPort(Environment.GetEnvironmentVariable(VaultEnvironmentVariables.Port))
                        .SetHost(Environment.GetEnvironmentVariable(VaultEnvironmentVariables.Host))
                        .SetSSL(false)
                        .Build();

    var rabbitMq = await vaultFacade.ReadRabbitMqSecretAsync();
    var connectionStringSecret = await vaultFacade.ReadFleetServiceConnectionStringSecretAsync();
    var cognito = await vaultFacade.ReadCognitoSecretAsync();

    builder.Configuration.AddInMemoryCollection(rabbitMq);
    builder.Configuration.AddInMemoryCollection(connectionStringSecret);
    builder.Configuration.AddInMemoryCollection(cognito);
}

var connectionString = builder.Configuration.GetConnectionString("FleetServiceDatabase");

builder.Services.AddDbContext<FleetServiceDbContext>(options =>
        options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(FleetServiceDbContext).Assembly.FullName)));

builder.Services.RegisterDatabase();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

builder.Services.Configure<RabbitMqSettingsDto>(builder.Configuration.GetRequiredSection("RabbitMQ"));
builder.Services.Configure<AwsCognitoSettingsDto>(builder.Configuration.GetRequiredSection("AWS:Cognito"));

try
{
    var cognitoConfiguration = builder.Configuration.GetRequiredSection("AWS:Cognito").Get<AwsCognitoSettingsDto>()!;
    builder.Services.AddAuthenticationAndJwtConfiguration(cognitoConfiguration);
}
catch (Exception ex)
{
    Log.Error(ex.Message);
}

try
{
    var rabbitMqSettings = builder.Configuration.GetRequiredSection("RabbitMQ").Get<RabbitMqSettingsDto>()!;
    builder.Services.AddRabbitMqConfiguration(rabbitMqSettings);
}
catch (Exception ex)
{
    Log.Error(ex.Message);
}

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddSingleton(EventStrategyConfiguration.GetGlobalSettingsConfiguration());

builder.Services.AddAuthorizationWithPolicies();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseFastEndpoints()
    .UseDefaultExceptionHandler()
    .UseSwaggerGen();

app.Run();

#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program { }
#pragma warning restore S1118 // Utility classes should not have public constructors