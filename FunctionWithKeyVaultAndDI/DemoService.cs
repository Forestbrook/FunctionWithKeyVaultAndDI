using Microsoft.Extensions.Configuration;
using System;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public class DemoService
{
    public DemoService(IConfiguration configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        // Get test secrets from the KeyVault:
        UserId = configuration[ConfigurationKeys.DatabaseUserId];
        TestSecret = configuration["TestSecret"];
    }

    public string TestSecret { get; }

    public string UserId { get; }
}