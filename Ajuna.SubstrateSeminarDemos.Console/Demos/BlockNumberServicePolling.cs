using System.Net.WebSockets;
using Serilog;
using SubstrateNET.RestClient;
using SubstrateNET.RestClient.Generated.Clients;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;

/// <summary>
/// Polling for the Block Number via the Service Layer 
/// </summary>
public class BlockNumberServicePolling: IDemoModule
{
        
    // Websocket and API addresses of the Service layer - You need Ajuna.SDK.Demos.RestService running for this console app to run
    private static String WebsocketUrl = "ws://localhost:61752/ws";
    private static String ServiceUrl = "http://localhost:61752/";

    
    public async Task ExecuteAsync(ILogger logger)
    {
        // Create BaseSubscriptionClient and connect
        var subscriptionClient = new BaseSubscriptionClient(new ClientWebSocket());
        await subscriptionClient.ConnectAsync(new Uri(WebsocketUrl), CancellationToken.None);
             
        // Create HttpClient
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(ServiceUrl)
        };
            
        // Create SystemControllerClient
        var systemControllerClient = new SystemControllerClient(httpClient, subscriptionClient);
            
        logger.Information($"Starting Number Value Polling");

        while (true)
        {
            var newNumber = await systemControllerClient.GetNumber();
            logger.Information($"Number is: {newNumber.Value}");
            Thread.Sleep(2000);
           
        }
            
        logger.Information($"Number Value Polling finished");    }
}