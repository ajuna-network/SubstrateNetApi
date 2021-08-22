using SubstrateNetApi;
using SubstrateNetApi.Model.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApiTest
{
    public class TestKeys
    {
        public static Account Alice => Account.Build(
                KeyType.Sr25519,
                Utils.HexToByteArray(
                    "0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011"),
                Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"));

        public static Account Zurich => Account.Build(
                KeyType.Ed25519,
                Utils.HexToByteArray(
                    "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

    }
}
