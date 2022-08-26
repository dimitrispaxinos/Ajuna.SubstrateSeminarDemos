using Serilog;
using SubstrateNET.NetApi.Generated;
using SubstrateNET.NetApi.Generated.Storage;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;


/// <summary>
/// Direct Polling for the Block Number
/// </summary>
public class BlockNumberDirectPolling: IDemoModule
{
    private static string NodeUrl = "ws://127.0.0.1:9944";

    public async Task ExecuteAsync(ILogger logger)
    {
        // Instantiate the client
        var client = new SubstrateClientExt(new Uri(NodeUrl));

        await client.ConnectAsync();
           
        // Display Client Connection Status after connecting
        logger.Information( client.IsConnected ? "Client connected successfully" : "Failed to connect to node. Exiting...");
            
        if (!client.IsConnected)
            return;
            
        logger.Information($"Starting Number Value Polling");

        while (true)
        {
            string parameters = SystemStorage.NumberParams();
            var num = await client.GetStorageAsync<NetApi.Model.Types.Primitive.U32>(parameters, new CancellationToken());                
            logger.Information($"Block Number is: {num.Value}");

            Thread.Sleep(2000);
        }
    }
}