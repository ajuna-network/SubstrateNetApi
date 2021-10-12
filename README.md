# SubstrateNetApi (NETStandard2.0)
*Just another Substrate .NET API* 
[![Build status](https://ci.appveyor.com/api/projects/status/jsei7yv376en17rr?svg=true)](https://ci.appveyor.com/project/darkfriend77/substratenetapi)
[![nuget](https://img.shields.io/nuget/v/SubstrateNetApi)](https://ci.appveyor.com/project/darkfriend77/substratenetapi/build/artifacts)
[![GitHub issues](https://img.shields.io/github/issues/darkfriend77/SubstrateNetApi.svg)](https://github.com/darkfriend77/SubstrateNetApi/issues)
[![license](https://img.shields.io/github/license/darkfriend77/SubstrateNetApi)](https://github.com/darkfriend77/SubstrateNetApi/blob/origin/LICENSE)
[![contributors](https://img.shields.io/github/contributors/darkfriend77/SubstrateNetApi)](https://github.com/darkfriend77/SubstrateNetApi/graphs/contributors)

## Substrate Version
**Important** This API is for the major release [**Substrate v3.0/0.9 – Apollo 14**](https://github.com/paritytech/substrate/releases/tag/v3.0.0), for releases [monthly-2021-10](https://github.com/paritytech/substrate/releases/tag/monthly-2021-10) and newer please refere to the [Ajuna.NetApi](https://github.com/ajuna-network/Ajuna.NetApi)

SubstrateNetApi is written in [NETStandard2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) to provide maximum compatibility for [Unity3D](https://docs.unity3d.com/2020.2/Documentation/Manual/dotnetProfileSupport.html). Feedback, constructive critisme and disscussions are welcome and will help us to improve the API! (Telegram: @darkfriend77, Discord: darkfriend77#3753)

If you enjoy using SubstrateNetApi consider supporting me at [buymeacoffee.com/darkfriend77](https://www.buymeacoffee.com/darkfriend77)

## General information

This project has been started by darkfriend77 as a proove of concept in october'20, the goal was to proove Unity3D compatibility towards substrate. [Substrate](https://substrate.dev/) is a modular framework that enables you to create purpose-built blockchains by composing custom or pre-built components. After succeding every aspect crucial for developing mobile games on unity against substrate, the project decided to work further with the API and apply for a first [open-grant](https://github.com/w3f/Grants-Program/blob/master/applications/dotmog.md) from the Web3 Foundation.

![ajuna-Header-1080p_with_logo](https://user-images.githubusercontent.com/17710198/136851142-2e4158ff-a6a7-4d26-9ea5-9833d62da3fa.png)

The project behind the SubstrateNetApi, is [Ajuna Network](https://ajuna.io/) and the flagship game [DOTMog](dotmog.com), backed by the swiss company BloGa Tech AG.

## Table of Content

1. [Home](https://github.com/JetonNetwork/SubstrateNetApi/wiki)  
1. [Requirements](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Requirements)  
1. [Installation](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Installation)
   - [NuGet](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Installation#substrate-in-net)
   - [Unity3D](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Installation#substrate-in-unity3d)  
1. [Usage](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Usage)  
1. [Types](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Types#types)
   - [Base Types](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Types#base-types)
   - [Enum Types](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Types#enum-types)
   - [Struct Types](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Types#struct-types)
1. [Wallet](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Wallet) 
   - [Create](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Wallet#create-a-new-wallet) 
   - [Mnemonic / Restore](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Wallet#mnemonic--restore)
   - [Wallet File](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Wallet#wallet-file)
1. [Extension](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Extension#extension)
1. [Testing](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Testing)  
   - [Guide](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Testing#guide)    
   - [Node-Template](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Testing#node-template)
1. [Project](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Project#project)  
   - [Substrate Version](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Project#substrate-version)
   - [Upgrading Substrate Version](https://github.com/JetonNetwork/SubstrateNetApi/wiki/Project#upgrading-substrate-version)

## Other Projects

Those projects are related to the SubstrateNetApi, by either implementing the API or being supported in the API.

- [SubstrateNetApiExt](https://github.com/JetonNetwork/SubstrateNetApiExt), SubstrateNetApi Extension Template a simple template for custom extension that integrate a pallet or set of pallets functionality for SubstrateNetApi.
- [Unity3DExample](https://github.com/dotmog/Unity3DExample), basic example of two test scenes implementing the substrate net api.
- [SubstrateUnityWalletSimple](https://github.com/dotmog/SubstrateUnityWalletSimple), simple test wallet integrating the substrate net wallet.
- [JtonNodeTemplate](https://github.com/JetonNetwork/JtonNodeTemplate), basic node-template currently used for the integration test and as development base for pallets.
- [jton-pallet-template](https://github.com/JetonNetwork/jton-pallet-template), Jeton Network Pallet Template, This is a template for a Jeton Network pallet which lives as its own crate so it can be imported into multiple runtimes. It is based on the "template" pallet that is included with the Substrate node template.

## Special Thanks
- https://github.com/gautamdhameja/sr25519-dotnet
- https://github.com/usetech-llc/polkadot_api_dotnet
