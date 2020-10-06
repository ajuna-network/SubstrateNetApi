# SubstrateNetApi (NETStandard2.0)
*Just another Substrate .NET API*  
[![Build status](https://ci.appveyor.com/api/projects/status/jsei7yv376en17rr?svg=true)](https://ci.appveyor.com/project/darkfriend77/substratenetapi)
[![nuget](https://img.shields.io/nuget/v/SubstrateNetApi)](https://img.shields.io/nuget/v/SubstrateNetApi)
![GitHub issues](https://img.shields.io/github/issues/darkfriend77/SubstrateNetApi.svg)
[![license](https://img.shields.io/github/license/darkfriend77/SubstrateNetApi)](https://img.shields.io/github/license/darkfriend77/SubstrateNetApi)
[![contributors](https://img.shields.io/github/contributors/darkfriend77/SubstrateNetApi)](https://img.shields.io/github/contributors/darkfriend77/SubstrateNetApi)

SubstrateNetApi is written in [NETStandard2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) to provide maximum compatibility for [Unity3D](https://docs.unity3d.com/2020.2/Documentation/Manual/dotnetProfileSupport.html).

## Table of Content

* Requirements
* Installation
* Usage
  - connecting
  - pallet storage data
  - pallet call
  - extrinsic (pallet author)

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

## 3. Usage

### Create a connection
```csharp
var WEBSOCKETURL = "wss://xyz.node.com"; // or local node ws://127.0.0.1:9944

using var client = new SubstrateClient(new Uri(WEBSOCKETURL));
client.RegisterTypeConverter(new MogwaiStructTypeConverter());
await client.ConnectAsync(cancellationToken);
```

### Access a pallet storage data

#### Example 1: Sudo Key (no parameter)

```csharp
// [Plain] Value: T::AccountId (from metaData)
var reqResult = await client.GetStorageAsync("Sudo", "Key", cancellationToken);
Console.WriteLine($"RESPONSE: '{reqResult}' [{reqResult.GetType().Name}]");
```
**OUTPUT 
```RESPONSE: '{"Address":"5GYZnHJ4dCtTDoQj4H5H9E727Ykv8NLWKtPAupEc3uJ89BGr","PublicKey":"xjCev8DKRhmK9W9PWJt82svJRhLQnZ5xsp5Z0cHy3mg="}' [AccountId]``` 

#### Example 2: System Account (Key: AccountId (public key))

```csharp
// [Map] Key: T::AccountId, Hasher: Blake2_128Concat, Value: AccountInfo<T::Index, T::AccountData> (from metaData)
var reqResult = await client.GetStorageAsync("System", "Account", "0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D", cancellationToken);
```
**OUTPUT  
```RESPONSE: '{"Nonce":4,"RefCount":0,"AccountData":{"Free":{"Value":{"Value":17665108313441014531489792}},"Reserved":{"Value":{"Value":0}},"MiscFrozen":{"Value":{"Value":0}},"FeeFrozen":{"Value":{"Value":0}}}}' [AccountInfo]```

### Access a pallet call

### Submit extrinsic (from pallet author)

## Special Thanks
- https://github.com/gautamdhameja/sr25519-dotnet
- https://github.com/usetech-llc/polkadot_api_dotnet
