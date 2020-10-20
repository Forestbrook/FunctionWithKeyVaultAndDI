using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Forestbrook.FunctionWithKeyVaultAndDI
{
    public class Function1
    {
        private readonly DemoService _demoService;

        public Function1(DemoService demoService)
        {
            _demoService = demoService ?? throw new ArgumentNullException(nameof(demoService));
        }

        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation(
                @$"-----------------------------------------------
C# Timer trigger function executed at: {DateTime.Now}
UserId: {_demoService.UserId}
TestSecret: {_demoService.TestSecret}
-----------------------------------------------");
        }
    }
}
