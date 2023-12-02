﻿using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts.DeploymentHandlers
{
#if !DOTNET35
    public class DeploymentEstimatorHandler<TContractDeploymentMessage> : DeploymentHandlerBase<TContractDeploymentMessage>, 
        IDeploymentEstimatorHandler<TContractDeploymentMessage> where TContractDeploymentMessage : ContractDeploymentMessage, new()
    {
    
        public DeploymentEstimatorHandler(ITransactionManager transactionManager):base(transactionManager)
        { 
        }


        public UniTask<HexBigInteger> EstimateGasAsync(TContractDeploymentMessage deploymentMessage = null)
        {
            if(deploymentMessage == null) deploymentMessage = new TContractDeploymentMessage();
            var callInput = DeploymentMessageEncodingService.CreateCallInput(deploymentMessage);
            if (TransactionManager.EstimateOrSetDefaultGasIfNotSet)
            {
                return TransactionManager.EstimateGasAsync(callInput);
            }

            return UniTask.FromResult(new HexBigInteger("0x0"));
        }
    }
#endif
}