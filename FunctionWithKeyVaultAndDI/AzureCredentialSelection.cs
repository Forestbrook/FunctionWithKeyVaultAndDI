namespace Forestbrook.FunctionWithKeyVaultAndDI;

public enum AzureCredentialSelection
{
    Default = 0,
    ManagedIdentity,
    VisualStudio
}