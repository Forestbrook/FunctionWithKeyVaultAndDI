using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Forestbrook.FunctionWithKeyVaultAndDI.Startup))]

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // Configure your services here.
        builder.Services.AddSingleton<DemoService>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        // local.settings.json are automatically loaded when debugging.
        // When running on Azure, values are loaded defined in app settings. See: https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
        builder.ConfigurationBuilder
            .AddAppSettingsJson(builder.GetContext())
            .AddEnvironmentVariables()
            .AddUserSecrets<Startup>(true)
            .AddAzureKeyVault()
            .Build();
    }
}