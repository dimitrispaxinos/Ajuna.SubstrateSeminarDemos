using Ajuna.NetApi;
using Ajuna.NetApi.Model.Extrinsics;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Schnorrkel.Keys;
using Serilog;
using SubstrateNET.NetApi.Generated;
using SubstrateNET.NetApi.Generated.Model.sp_core.crypto;
using SubstrateNET.NetApi.Generated.Model.sp_runtime.multiaddress;

namespace Ajuna.SubstrateSeminarDemos.Console.Demos;

/// <summary>
/// Balance Transfer Extrinsic Execution
/// </summary>
public class ExtrinsicExecution :IDemoModule
{
    private static string NodeUrl = "ws://127.0.0.1:9944";
    // Secret Key URI `//Alice` is account:
    // Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
    // Public key(hex):  0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
    // Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
    // SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
    public static MiniSecret MiniSecretAlice => new MiniSecret(
        Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"),
        ExpandMode.Ed25519);

    public static Account Alice => Account.Build(KeyType.Sr25519, 
        MiniSecretAlice.ExpandToSecret().ToBytes(),
        MiniSecretAlice.GetPair().Public.Key);

    // Secret Key URI `//Bob` is account:
    // Secret seed:      0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89
    // Public key(hex):  0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
    // Account ID:       0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
    // SS58 Address:     5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty
    public static MiniSecret MiniSecretBob => new MiniSecret(
        Utils.HexToByteArray("0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89"),
        ExpandMode.Ed25519);

    public static Account Bob => Account.Build(KeyType.Sr25519, 
        MiniSecretBob.ExpandToSecret().ToBytes(),
        MiniSecretBob.GetPair().Public.Key);
    
    public async Task ExecuteAsync(ILogger logger)
    {  
        // Instantiate the client
        var client = new SubstrateClientExt(new Uri(NodeUrl));
        await client.ConnectAsync();
        
        // Display Client Connection Status after connecting
        logger.Information( client.IsConnected ? "Client connected successfully" : "Failed to connect to node. Exiting...");

        if (!client.IsConnected)
            return;
        
        var accountAlice = new AccountId32();
        accountAlice.Create(Utils.GetPublicKeyFrom(Alice.Value));

        var accountBob = new AccountId32();
        accountBob.Create(Utils.GetPublicKeyFrom(Bob.Value));

        // Get Alice's Balance
        var accountInfoAlice = await client.SystemStorage.Account(accountAlice, CancellationToken.None);
        logger.Information($"Alice Free Balance before transaction = {accountInfoAlice.Data.Free.Value.ToString()}");

        // Get Bob's Balance
        var accountInfoBob = await client.SystemStorage.Account(accountBob, CancellationToken.None);
        logger.Information($"Bob Free Balance before transaction = {accountInfoBob.Data.Free.Value.ToString()}");

        // Instantiate a MultiAddress for Bob
        var multiAddressBob = new EnumMultiAddress();
        multiAddressBob.Create(MultiAddress.Id, accountBob);

        // Amount to be transferred
        var amount = new BaseCom<U128>();
        amount.Create(190000);

        // Create Extrinsic Method to be transmitted
        var extrinsicMethod =
            SubstrateNET.NetApi.Generated.Storage.BalancesCalls.Transfer(multiAddressBob, amount);
        
        // Alice to Bob Transaction
        await client.Author.SubmitExtrinsicAsync(extrinsicMethod, Alice, new ChargeAssetTxPayment(0, 0), 128,
            CancellationToken.None);
        
        Thread.Sleep(10000);

        accountInfoAlice = await client.SystemStorage.Account(accountAlice, CancellationToken.None);
        logger.Information($"Alice Free Balance after transaction = {accountInfoAlice.Data.Free.Value}");

        accountInfoBob = await client.SystemStorage.Account(accountBob, CancellationToken.None);
        logger.Information($"Bob Free Balance after transaction = {accountInfoBob.Data.Free.Value}");
    }
}
