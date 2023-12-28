using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public static class FunctionHelper
{
    public static IConfigurationBuilder AddAppSettingsJson(this IConfigurationBuilder builder, HostBuilderContext hostingContext)
    {
        // REMARKS:
        // hostingContext.HostingEnvironment.EnvironmentName defaults to Production, but when local.settings.json is loaded, it defaults to Development.
        // To change the default, set the environment variable AZURE_FUNCTIONS_ENVIRONMENT.
        // For local testing, you can set AZURE_FUNCTIONS_ENVIRONMENT in Properties/launchSettings.json:
        // e.g.: "environmentVariables": { "AZURE_FUNCTIONS_ENVIRONMENT": "Staging" },

        var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);
        return builder;
    }

    /// <summary>
    /// Make sure to add to your appsettings.json: "KeyVaultName": "your-key-vault-name"
    /// </summary>
    public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder builder)
    {
        var configuration = builder.Build();
        var keyVaultUri = configuration.CreateKeyVaultUri();
        var keyVaultCredential = configuration.CreateKeyVaultCredential();
        return builder.AddAzureKeyVault(keyVaultUri, keyVaultCredential);
    }

    private static TokenCredential CreateKeyVaultCredential(this IConfiguration configuration)
    {
        // WARNING: Make sure to give the App in the Azure Portal access to the KeyVault.
        //          In the Identity tab: System Assigned part: turn Status On.
        //          In the KeyVault: Access control (IAM) > Add role assignment > Select role: Key Vault Secret User => Members tab: Assign access to: Managed identity and with +Select members select your functions app.
        // When running on Azure, you do NOT need to set the KeyVaultTenantId.
        var keyVaultTenantId = configuration[ConfigurationKeys.KeyVaultTenantId];
        if (string.IsNullOrEmpty(keyVaultTenantId))
            return new DefaultAzureCredential();

        // When debugging local from VisualStudio AND the TenantId differs from default AZURE_TENANT_ID (in Windows settings/environment variables),
        // you can store KeyVaultTenantId= in appsettings or in UserSecrets and read it here from the configuration (as done above)
        // See also: https://www.forestbrook.net/docs/azure/functionwithkeyvaultanddi.html#local-testingdebugging-in-visual-studio
        var options = new DefaultAzureCredentialOptions { VisualStudioTenantId = keyVaultTenantId };
        return new DefaultAzureCredential(options);
    }

    private static Uri CreateKeyVaultUri(this IConfiguration configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        var keyVaultName = configuration[ConfigurationKeys.KeyVaultName];
        if (string.IsNullOrEmpty(keyVaultName))
            throw new InvalidOperationException($"Missing configuration setting {ConfigurationKeys.KeyVaultName}");

        return new Uri($"https://{keyVaultName}.vault.azure.net/");
    }
}