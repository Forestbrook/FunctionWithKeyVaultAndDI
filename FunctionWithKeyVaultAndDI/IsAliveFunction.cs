using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public class IsAliveFunction
{
    // Test local: http://localhost:7150/api/IsAliveFunction?TestQuery=TestQueryValue (port specified in Properties/launchSettings.json).
    // Test on Azure: https://TODO-your-function-name-.azurewebsites.net/api/IsAliveFunction?TestQuery=TestQueryValue
    private const string IsRunningMessage = "Forestbrook Function is running. Version:";
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger _logger;

    public IsAliveFunction(IConfiguration configuration, IHostEnvironment hostEnvironment, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        _logger = loggerFactory.CreateLogger<IsAliveFunction>();
    }

    [Function(nameof(IsAliveFunction))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation($"C# HTTP trigger function {nameof(IsAliveFunction)} is processing a request.");

        // Execute async tasks:
        await Task.CompletedTask;

        var version = typeof(Program).Assembly.GetName().Version?.ToString(3);
        var sb = new StringBuilder($"{DateTime.Now:d-M-yyyy H:mm:ss} - {IsRunningMessage} {version}<br/>");
        sb.Append("<br/>");
        sb.Append($"EnvironmentName = {_hostEnvironment.EnvironmentName}<br/>");
        sb.Append($"IsDevelopment: {_hostEnvironment.IsDevelopment()}<br/>");
        sb.Append($"IsProduction: {_hostEnvironment.IsProduction()}<br/>");
        sb.Append($"ApplicationName = {_hostEnvironment.ApplicationName}<br/>");
        sb.Append($"ContentRootPath = {_hostEnvironment.ContentRootPath}<br/>");
        sb.Append("<br/>");
        sb.Append("<b>Test settings to show the priority in the configuration providers:</b><br/>");
        sb.Append($"TestSetting1 = {_configuration["TestSetting1"]}<br/>");
        sb.Append($"TestSetting2 = {_configuration["TestSetting2"]}<br/>");
        sb.Append($"TestSetting3 = {_configuration["TestSetting3"]}<br/>");
        sb.Append($"TestSetting4 = {_configuration["TestSetting4"]}<br/>");
        sb.Append("<br/>");
        if (req.Query.AllKeys.Length > 0)
        {
            sb.Append("QUERY VALUES:<br/>");
            foreach (var key in req.Query.AllKeys)
                sb.Append($"{key} = {req.Query[key]}<br/>");
        }

        // Show we're alive:
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        response.WriteString(sb.ToString());
        return response;
    }
}