namespace Forestbrook.FunctionWithKeyVaultAndDI
{
    public class DemoService
    {
        public DemoService(string userId, string testSecret)
        {
            UserId = userId;
            TestSecret = testSecret;
        }

        public string TestSecret { get; }

        public string UserId { get; }
    }
}
