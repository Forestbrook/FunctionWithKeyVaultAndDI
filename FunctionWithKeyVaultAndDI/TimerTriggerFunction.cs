using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public class TimerTriggerFunction
{
    private readonly DemoService _demoService;

    public TimerTriggerFunction(DemoService demoService)
    {
        _demoService = demoService ?? throw new ArgumentNullException(nameof(demoService));
    }

    [FunctionName(nameof(TimerTriggerFunction))]
    public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
    {
        var sb = new StringBuilder();
        sb.AppendLine("-----------------------------------------------");
        sb.AppendLine($"C# Timer trigger function executed at: {DateTime.Now}");
        sb.AppendLine($"UserId: {_demoService.UserId}");
        sb.AppendLine($"TestSecret: {_demoService.TestSecret}");
        sb.AppendLine("-----------------------------------------------");
        log.LogInformation(sb.ToString());
    }
}