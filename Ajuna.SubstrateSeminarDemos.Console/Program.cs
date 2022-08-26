using Ajuna.NetApi;
using Ajuna.SubstrateSeminarDemos.Console.Demos;
using Serilog;

namespace Ajuna.SubstrateSeminarDemos.Console
{
    internal static class Program
    {
        private static string _nodeUrl = "ws://127.0.0.1:9944";

        private static readonly ILogger Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        public static async Task Main(string[] args)
        {
            // Ajuna.NetApi Simple Connectivity Demo
            var netApiDemo = new NetApiDemo();
            
            // Direct Polling for the Block Number
            var blockNumberDirectPollingDemo = new BlockNumberDirectPolling();
            
            // Direct Subscription for the Block Number
            var blockNumberDirectSubscriptionDemo = new BlockNumberDirectSubscription();
            
            // Subscribing to the Block Number changes via the Service Layer 
            var blockNumberServiceSubscriptionDemo = new BlockNumberServiceSubscription();
            
            // Decide which example you would like to execute
            var blockNumberServicePollingDemo = new BlockNumberServicePolling();
            
            // Balance Transfer Extrinsic Execution
            var extrinsicExecutionDemo = new ExtrinsicExecution();

            // await netApiDemo.ExecuteAsync(Logger);
            // await blockNumberDirectPollingDemo.ExecuteAsync(Logger);
            // await blockNumberDirectSubscriptionDemo.ExecuteAsync(Logger);
            // await extrinsicExecutionDemo.ExecuteAsync(Logger);
            // await blockNumberServiceSubscriptionDemo.ExecuteAsync(Logger);
            await blockNumberServicePollingDemo.ExecuteAsync(Logger);

        }
    }
}
