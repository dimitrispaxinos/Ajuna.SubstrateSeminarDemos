using System.Net.WebSockets;
using Ajuna.NetApi;
using Ajuna.ServiceLayer.Model;
using Serilog;
using Serilog.Core;
using SubstrateNET.RestClient;
using SubstrateNET.RestClient.Generated.Clients;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;

/// <summary>
/// Subscribing to the Block Number changes via the Service Layer 
/// </summary>
public class BlockNumberServiceSubscription: IDemoModule
{
    // Websocket and API addresses of the Service layer - You need Ajuna.SDK.Demos.RestService running for this console app to run
    private static String WebsocketUrl = "ws://localhost:61752/ws";
    private static String ServiceUrl = "http://localhost:61752/";

    
    public async Task ExecuteAsync(ILogger logger)
    {
        // Create BaseSubscriptionClient and connect
        var subscriptionClient = new BaseSubscriptionClient(new ClientWebSocket());
        await subscriptionClient.ConnectAsync(new Uri(WebsocketUrl), CancellationToken.None);
             
        // Assign Generic Handler for Storage Change
        subscriptionClient.OnStorageChange = (message) => HandleChange(message,logger);

        // Create HttpClient
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(ServiceUrl)
        };
            
        // Create SystemControllerClient
        var systemControllerClient = new SystemControllerClient(httpClient, subscriptionClient);

        // Subscribe to Number Changes
        var subscribedSuccessfully = await systemControllerClient.SubscribeNumber();
            
        // Continue only if Subscription has succeeded
        if (subscribedSuccessfully)
        {
            logger.Information("Successfully Subscribed. Now listening for Storage Changes...");
            logger.Information("Press ESC to exit");
        }
        else
        {
            logger.Information("Subscription failed. Exiting...");
            return;
        }

        while (true)
        {
            await subscriptionClient.ReceiveNextAsync(CancellationToken.None);
      }
         
        // Close Websocket Connection 
       // await subscriptionClient.CloseAsync(WebSocketCloseStatus.Empty,"",CancellationToken.None);
    }
    
    private static void HandleChange(StorageChangeMessage message, ILogger logger)
    {
        var primitiveBlockNumber = new NetApi.Model.Types.Primitive.U32();
        primitiveBlockNumber.Create(Utils.HexToByteArray(message.Data));

        logger.Information("New Block Number: " + primitiveBlockNumber.Value);
    }
}