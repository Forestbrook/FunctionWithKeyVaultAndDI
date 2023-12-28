using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Forestbrook.FunctionWithKeyVaultAndDI;

public class TimerTriggerFunction
{
    private const string TimerSchedule = "0 */5 * * * *"; // Every 5 minutes
    private readonly DemoService _demoService;
    private readonly ILogger _logger;

    public TimerTriggerFunction(DemoService demoService, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _demoService = demoService ?? throw new ArgumentNullException(nameof(demoService));
        _logger = loggerFactory.CreateLogger<TimerTriggerFunction>();
    }

    [Function(nameof(TimerTriggerFunction))]
    public Task Run([TimerTrigger(TimerSchedule)] TimerInfo timerInfo)
    {
        var sb = new StringBuilder();
        sb.AppendLine("-----------------------------------------------");
        sb.AppendLine($"C# Timer trigger function executed at: {DateTime.Now}. IsPastDue: {timerInfo.IsPastDue}");
        sb.AppendLine($"Next timer schedule at: {timerInfo.ScheduleStatus?.Next}");
        sb.AppendLine($"UserId: {_demoService.UserId}");
        sb.AppendLine($"TestSecret: {_demoService.TestSecret}");
        sb.AppendLine("-----------------------------------------------");
        _logger.LogInformation(sb.ToString());

        // Short delay to prevent completion to mess up with the logging output.
        return Task.Delay(100);
    }
}