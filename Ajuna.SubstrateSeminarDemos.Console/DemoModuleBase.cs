using Ajuna.NetApi;
using SubstrateNET.NetApi.Generated;

namespace Ajuna.SubstrateSeminarDemos.Console;

public class DemoModuleBase
{
    protected async Task<SubstrateClient> GetClientAndConnectAsync()
    {
        var uri = new Uri("ws://127.0.0.1:9944");
        SubstrateClient client = new SubstrateClient(uri);
        await client.ConnectAsync();
        return client;

    }
    
    protected async Task<SubstrateClientExt> GetClientExtAndConnectAsync()
    {
        var uri = new Uri("ws://127.0.0.1:9944");
        var client = new SubstrateClientExt(uri);
        await client.ConnectAsync();
        return client;

    }
}