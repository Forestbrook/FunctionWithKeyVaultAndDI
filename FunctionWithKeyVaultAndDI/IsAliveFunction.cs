using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public class IsAliveFunction
{
    // Test local: http://localhost:7071/api/IsAliveFunction
    // Test on Azure: https://TODO-your-function-name-.azurewebsites.net/api/IsAliveFunction
    private const string IsRunningMessage = "Forestbrook Function is running. Version:";

    [FunctionName(nameof(IsAliveFunction))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        // Execute async tasks:
        await Task.CompletedTask;

        var version = GetType().Assembly.GetName().Version?.ToString(3);
        var sb = new StringBuilder($"{DateTime.Now:d-M-yyyy H:mm:ss} - {IsRunningMessage} {version}");
        if (req.Query.Count > 0)
        {
            sb.Append("<br/><br/>QUERY VALUES:<br/>");
            foreach (var (key, value) in req.Query)
                sb.Append($"{key} = {value}<br/>");
        }

        // Show we're alive:
        return new ContentResult()
        {
            Content = sb.ToString(),
            ContentType = "text/html",
        };
    }
}