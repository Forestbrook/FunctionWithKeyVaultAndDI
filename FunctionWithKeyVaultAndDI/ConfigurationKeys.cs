namespace Forestbrook.FunctionWithKeyVaultAndDI;

public static class ConfigurationKeys
{
    /// <summary>
    /// StorageConnectionString for Azure Function
    /// </summary>
    public const string AzureWebJobsStorage = "AzureWebJobsStorage";

    public const string DatabaseUserId = "DbCredentials:UserId";
    public const string KeyVaultName = "KeyVaultName";
    public const string KeyVaultTenantId = "KeyVaultTenantId";
    public const string StorageConnectionString = "StorageCredentials:ConnectionString";
}