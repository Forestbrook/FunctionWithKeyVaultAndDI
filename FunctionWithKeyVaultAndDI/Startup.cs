using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Forestbrook.FunctionWithKeyVaultAndDI.Startup))]
namespace Forestbrook.FunctionWithKeyVaultAndDI
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(s => new DemoService(_configuration["DbCredentials:UserId"], _configuration["TestSecret"]));
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder fnConfigBuilder)
        {
            var context = fnConfigBuilder.GetContext();
            var builder = fnConfigBuilder.ConfigurationBuilder;
            builder.AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), true, false)
                .AddEnvironmentVariables();

            // Add the Key Vault:
            // WARNING: Make sure to give the Function App in the Azure Portal access to the KeyVault.
            //          In the Identity tab: System Assigned part: turn Status On and copy the Object ID.
            //          In the KeyVault: Access Policies > Add Access Policy > Secret Permissions Get, List and Select Principal: Object ID copied above.
            var builtConfig = builder.Build();
            var keyVaultUri = $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/";
            builder.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());

            _configuration = builder.Build();
        }
    }
}
