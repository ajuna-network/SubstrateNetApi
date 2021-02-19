# SubstrateNetApi (NETStandard2.0)
*Just another Substrate .NET API*  
[![Build status](https://ci.appveyor.com/api/projects/status/jsei7yv376en17rr?svg=true)](https://ci.appveyor.com/project/darkfriend77/substratenetapi)
[![nuget](https://img.shields.io/nuget/v/SubstrateNetApi)](https://ci.appveyor.com/project/darkfriend77/substratenetapi/build/artifacts)
[![GitHub issues](https://img.shields.io/github/issues/darkfriend77/SubstrateNetApi.svg)](https://github.com/darkfriend77/SubstrateNetApi/issues)
[![license](https://img.shields.io/github/license/darkfriend77/SubstrateNetApi)](https://github.com/darkfriend77/SubstrateNetApi/blob/origin/LICENSE)
[![contributors](https://img.shields.io/github/contributors/darkfriend77/SubstrateNetApi)](https://github.com/darkfriend77/SubstrateNetApi/graphs/contributors)

SubstrateNetApi is written in [NETStandard2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) to provide maximum compatibility for [Unity3D](https://docs.unity3d.com/2020.2/Documentation/Manual/dotnetProfileSupport.html). Feedback, constructive critisme and disscussions are welcome and will help us to improve the API!

If you enjoy using SubstrateNetApi consider supporting me at [buymeacoffee.com/darkfriend77](https://www.buymeacoffee.com/darkfriend77)

## Table of Content

* Requirements
* Installation
  - Substrate in .NET
  - Substrate in Unity3D
* Architecture
* Basic Usage
  - Create a connection
    - Add a custom type
  - Access MetaData as JSON
  - Access a pallet storage data
    - Example 1: Sudo Key (no parameter)
    - Example 2: System Account (with parameter)
  - Access a pallet call
  - Submit extrinsic
* Advanced Usage
  - Subscribe & unsubscribe
* Wallet
  - Create
  - Unlock
  - Recover

## Requirements

* Windows, MacOS or Linux
  - [Visual Studio 2019 Community](https://visualstudio.microsoft.com/de/vs/) or [Visual Studio Code](https://code.visualstudio.com/) for best .NET Core support
  - [.NET Core](https://www.microsoft.com/net/download/core)

## Installation

### Substrate in .NET
Add NuGet package https://www.nuget.org/packages/SubstrateNetApi/

### Substrate in Unity3D

#### Method A:
Added the dependency needed, there is an example project (https://github.com/darkfriend77/Unity3DExample), which already imported the necessary dependencies. 

![NuGet Dependencies](https://github.com/darkfriend77/SubstrateNetApi/raw/origin/images/dependencies.png)

#### Method B:
*Currently there is an issue where NuGetForUnity, will pull to many dependencies and break the project.*
- Download *NuGetForUnity.x.y.z.unitypackage* Link https://github.com/GlitchEnzo/NuGetForUnity/releases
- Open our unity project
- *Asset > Import package > Custom package*
- Choose downloaded NuGetForUnity.x.y.z.unitypackage
- Installation for **NuGetForUnity** https://github.com/GlitchEnzo/NuGetForUnity
- *NuGet > ManageNuGet Packages*
- *Search* **SubstrateNetApi** install

## Architecture

### Example Architecture with SubstrateNetApi

Basic example of a client and a substrate node environment with the SubstrateNetApi.

![Basic Architecture](https://github.com/dotmog/SubstrateNetApi/raw/origin/images/basic_architecture.png)

## Usage

### Create a connection
```csharp
var WEBSOCKETURL = "wss://xyz.node.com"; // or local node ws://127.0.0.1:9944
using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
await client.ConnectAsync(cancellationToken);
```
#### Add a custom type

To be able to add a custom type you need to first add it as a class inheriting [IType](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/IType.cs), or you can use following abstract classes:

- [BaseType](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/BaseType.cs)
  BaseType is for basic implementation of a type that represents one value, like a [Hash](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/Base/Hash.cs) or a [U8](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/Base/U8.cs).
- [StructType](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/StructType.cs)
  StructType is for complex types composed of other Types, for example [AccountInfo](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/Struct/AccountInfo.cs)
- [EnumType](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/EnumType.cs)
  EnumType is for enums like [DispatchClass](https://github.com/dotmog/SubstrateNetApi/blob/origin/SubstrateNetApi/Model/Types/Enum/DispatchClass.cs).

```csharp
var WEBSOCKETURL = "wss://xyz.node.com"; // or local node ws://127.0.0.1:9944
using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
client.RegisterTypeConverter(new GenericTypeConverter<MogwaiStruct>()); // custom types
client.RegisterTypeConverter(new GenericTypeConverter<MogwaiBios>());  // custom types
await client.ConnectAsync(cancellationToken);
```


### Access MetaData as JSON (chain specific)

```csharp
// MetaData 
Console.WriteLine(client.MetaData.Serialize());
```

### Access a pallet storage data

#### Example 1: Sudo Key (no parameter)

```csharp
// [Plain] Value: T::AccountId (from metaData)
var reqResult = await client.GetStorageAsync("Sudo", "Key", cancellationToken);
Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult.GetType().Name}]");
```
```RESPONSE: '"5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH"' [AccountId]``` 

#### Example 2: System Account (Key: AccountId (public key))

```csharp
// [Map] Key: T::AccountId, Hasher: Blake2_128Concat, Value: AccountInfo<T::Index, T::AccountData> (from metaData)
var reqResult = await client.GetStorageAsync("System", "Account", new [] {Utils.Bytes2HexString(Utils.GetPublicKeyFrom(address))}, cancellationToken);
Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult.GetType().Name}]");
```
```RESPONSE: '{"Nonce":{"Value":15},"Consumers":{"Value":0},"Providers":{"Value":1},"AccountData":{"Free":{"Value":1998766600579252800594},"Reserved":{"Value":0},"MiscFrozen":{"Value":0},"FeeFrozen":{"Value":0}}}' [AccountInfo]```

### Access a pallet call

```csharp
var systemName = await client.System.NameAsync(cancellationToken);
```
```RESPONSE: 'DOTMog Node' [String]```

### Submit extrinsic (from pallet author)

```csharp
// Dest: <T::Lookup as StaticLookup>::Source, Value: Compact<T::Balance>
var balanceTransfer = ExtrinsicCall.BalanceTransfer("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH", BigInteger.Parse("100000000000"));
var reqResult = await client.Author.SubmitExtrinsicAsync(balanceTransfer, accountZurich, 0, 64, cancellationToken);
```
```RESPONSE: '"0xB1435DA6A0F2C9C00E1AC0FAD7EBD2515B8AACDA12F27384A2148C556FEE627A"' [Hash]```

### Subscribe and unsubscribe with registering a call back

```csharp
// create a call back action with the expected type
Action<ExtrinsicStatus> actionExtrinsicUpdate = (extrinsicUpdate) => Console.WriteLine($"CallBack: {extrinsicUpdate}");
// create the extrinsic parameters
var balanceTransfer = ExtrinsicCall.BalanceTransfer("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", 1000);
// submit and subscribe to the extrinsic
var subscriptionId = await client.Author.SubmitAndWatchExtrinsicAsync(actionExtrinsicUpdate, balanceTransfer, accountDMOG_GALxeh, 0, 64, cancellationToken);
// wait for extrinsic pass into a finalized block
Thread.Sleep(60000);
// unsubscribe
var reqResult = await client.Author.UnwatchExtrinsicAsync(subscriptionId, cancellationToken);
```

## Wallet

SubstrateNetWallet is a Wallet buit on top of the SubstrateNetApi, it offers common functionalities. 
**This is an implementation in progress, feedback is welcome!**

- Key derivation, currently only for ED25519
- Private keystore with AES encryption (please verify code before using in a productive environment)
- Sign message & verify message
- Transfer balance
- realtime (subscription) updated newHeads and finalizedHeads
- realtime (subscription) updated extrinsicUpdate

### Create a new wallet

### Unlock a wallet

### Recover a wallet

### Add subscriptions

## Special Thanks
- https://github.com/gautamdhameja/sr25519-dotnet
- https://github.com/usetech-llc/polkadot_api_dotnet
