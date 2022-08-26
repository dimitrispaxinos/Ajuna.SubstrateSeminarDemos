using Ajuna.NetApi;
using Ajuna.NetApi.Model.Rpc;
using Serilog;
using SubstrateNET.NetApi.Generated;
using SubstrateNET.NetApi.Generated.Storage;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;

/// <summary>
/// Direct Subscription for the Block Number
/// </summary>
public class BlockNumberDirectSubscription: IDemoModule
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

        // Display Client Connection Status after connecting
        logger.Information( client.IsConnected ? "Client connected successfully" : "Failed to connect to node. Exiting...");

        if (!client.IsConnected)
            return;
        
        logger.Information($"Subscribing to Block Number Changes");

        // Subscribe to the changes of System.Number by registering a Callback for each Number Change  
        await client.SubscribeStorageKeyAsync(SystemStorage.NumberParams(),
            CallBackNumberChange, CancellationToken.None);

        System.Console.ReadLine();
    }
    
    /// <summary>
    /// Called on any number change.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="storageChangeSet">The storage change set.</param>
    private static void CallBackNumberChange(string subscriptionId, StorageChangeSet storageChangeSet)
    {
        if (storageChangeSet.Changes == null 
            || storageChangeSet.Changes.Length == 0 
            || storageChangeSet.Changes[0].Length < 2)
        {
            System.Console.WriteLine("Couldn't update account information. Please check 'CallBackAccountChange'");
            return;
        }

        var hexString = storageChangeSet.Changes[0][1];
        
        var primitiveBlockNumber = new NetApi.Model.Types.Primitive.U32();
        primitiveBlockNumber.Create(Utils.HexToByteArray(hexString));

        System.Console.WriteLine("New Number Change: " + primitiveBlockNumber.Value);
    }
    
    private static string GetClientConnectionStatus(SubstrateClient client)
    {
        return client.IsConnected ? "Connected" : "Not connected";
    }
}