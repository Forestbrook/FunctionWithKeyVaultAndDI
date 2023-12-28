using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

internal class Program
{
    static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices(Configure)
            .Build();

        await host.RunAsync();
    }

    private static void Configure(HostBuilderContext context, IServiceCollection services)
    {
        // Configure your services here:
        services.AddSingleton<DemoService>();
    }

    private static void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder appConfigBuilder)
    {
        // When running/debugging locally, local.settings.json is automatically loaded
        // Environment defaults to Development
        // To change the environment set it in Properties/launchSettings.json: "environmentVariables": { "AZURE_FUNCTIONS_ENVIRONMENT": "Staging" },

        // When running on Azure, values defined in your Function App section Configuration => Application Settings are loaded as Environment Variables.
        // Environment defaults to Production
        // To change the environment set it with AZURE_FUNCTIONS_ENVIRONMENT in Application Settings

        // See also: Manage your function app: https://learn.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
        // All possible settings see App settings reference for Azure Functions: https://learn.microsoft.com/en-us/azure/azure-functions/functions-app-settings

        appConfigBuilder.AddAppSettingsJson(hostingContext);
        appConfigBuilder.AddEnvironmentVariables();
        appConfigBuilder.AddUserSecrets<Program>(optional: true);
        appConfigBuilder.AddAzureKeyVault();
    }
}