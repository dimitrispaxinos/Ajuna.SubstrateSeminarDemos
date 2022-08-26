using Ajuna.NetApi;
using Serilog;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;

/// <summary>
/// Simple Connection Demo to a Node using SubstrateClient
/// </summary>
public class NetApiDemo : IDemoModule
{
    public async  Task ExecuteAsync(ILogger logger)
    {
        var uri = new Uri("ws://127.0.0.1:9944");
        SubstrateClient client = new SubstrateClient(uri);
        
        logger.Information($"client.IsConnected: {client.IsConnected}");

        await client.ConnectAsync();
        
        logger.Information($"client.IsConnected: {client.IsConnected}");

        // Show SpecName
        logger.Information($"SpecName: {client.RuntimeVersion.SpecName}");
        
        // Show ImplName
        logger.Information($"ImplName: {client.RuntimeVersion.ImplName}");
        
        // Show Metadata
        //logger.Information($"MetaData:\n {JsonConvert.SerializeObject(client.MetaData)}");
        
        System.Console.ReadLine();
    }
}