﻿using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;

namespace Nethereum.RPC.Eth
{
    public interface IEthGetProof
    {
        BlockParameter DefaultBlock { get; set; }

        RpcRequest BuildRequest(string address, string[] storageKeys, BlockParameter block, object id = null);
        UniTask<AccountProof> SendRequestAsync(string address, string[] storageKeys, object id = null);
        UniTask<AccountProof> SendRequestAsync(string address, string[] storageKeys, BlockParameter block, object id = null);
    }
}