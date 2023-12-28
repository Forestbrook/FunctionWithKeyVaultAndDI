namespace Forestbrook.FunctionWithKeyVaultAndDI;

public static class ConfigurationKeys
{
    public const string AzureFunctionsEnvironment = "AZURE_FUNCTIONS_ENVIRONMENT";

    /// <summary>
    /// StorageConnectionString for Azure Function
    /// </summary>
    public const string AzureWebJobsStorage = "AzureWebJobsStorage";

    public const string DatabaseUserId = "DbCredentials:UserId";
    public const string KeyVaultName = "KeyVaultName";
    public const string KeyVaultTenantId = "KeyVaultTenantId";
}