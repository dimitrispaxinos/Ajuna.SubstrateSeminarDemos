using Serilog;
using SubstrateNET.NetApi.Generated;
using SubstrateNET.NetApi.Generated.Storage;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;


/// <summary>
/// Direct Polling for the Block Number
/// </summary>
public class BlockNumberDirectPolling:  DemoModuleBase, IDemoModule
{
    private static string NodeUrl = "ws://127.0.0.1:9944";

    public async Task ExecuteAsync(ILogger logger)
    {
        // Instantiate the client
        var client = await GetClientExtAndConnectAsync();

        if (!client.IsConnected)
            return;
            
        logger.Information($"Starting Number Value Polling");

        while (true)
        {
            var scaleNum = await client.SystemStorage.Number(CancellationToken.None);
            logger.Information("Block Number: {BlockNumber}", scaleNum.Value);
            Thread.Sleep(2000);
        }
    }
}