using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args)
            .ConfigureFunctionsWebApplication();

        ConfigureHostConfiguration(builder.Configuration, builder.Environment);
        ConfigureServices(builder);
        await builder.Build().RunAsync();
    }

    private static void ConfigureServices(IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        // Configure your services here:
        services.AddSingleton<DemoService>();
    }

    private static void ConfigureHostConfiguration(IConfigurationBuilder appConfigBuilder, IHostEnvironment hostEnvironment)
    {
        // When running/debugging locally, local.settings.json is automatically loaded
        // Environment defaults to Development
        // To change the environment set it in Properties/launchSettings.json: "environmentVariables": { "AZURE_FUNCTIONS_ENVIRONMENT": "Staging" },

        // When running on Azure, values defined in your Function App section Configuration => Application Settings are loaded as Environment Variables.
        // Environment defaults to Production
        // To change the environment set it with AZURE_FUNCTIONS_ENVIRONMENT in Application Settings

        // See also: Manage your function app: https://learn.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
        // All possible settings see App settings reference for Azure Functions: https://learn.microsoft.com/en-us/azure/azure-functions/functions-app-settings

        appConfigBuilder.AddAppSettingsJson(hostEnvironment);
        appConfigBuilder.AddEnvironmentVariables();
        appConfigBuilder.AddUserSecrets<Program>(optional: true);
        // NOTE: You can use the default AzureCredentialSelection when debugging, but in Visual Studio it might take too long to authenticate, so you can use the VisualStudio credential selection when debugging locally.
#if DEBUG
        appConfigBuilder.AddAzureKeyVault(AzureCredentialSelection.VisualStudio);
#else
        appConfigBuilder.AddAzureKeyVault();
#endif
    }
}