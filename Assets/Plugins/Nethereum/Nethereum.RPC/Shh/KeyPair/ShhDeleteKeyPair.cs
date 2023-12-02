﻿using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Shh.KeyPair
{
    public class ShhDeleteKeyPair : GenericRpcRequestResponseHandlerParamString<bool>, IShhDeleteKeyPair
    {
        public ShhDeleteKeyPair(IClient client) : base(client, ApiMethods.shh_deleteKeyPair.ToString())
        {
        } 
    }
}
