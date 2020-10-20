using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Forestbrook.FunctionWithKeyVaultAndDI
{
    public class Function1
    {
        public Function1()
        {

        }

        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
