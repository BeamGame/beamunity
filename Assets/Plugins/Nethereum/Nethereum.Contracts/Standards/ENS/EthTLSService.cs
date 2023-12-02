﻿using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.Contracts.Services;
using Nethereum.Contracts.Standards.ENS.ETHRegistrarController.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts.Standards.ENS
{
    public class EthTLSService
    {
        private readonly IEthApiContractService _ethApiContractService;

        public EthTLSService(IEthApiContractService ethApiContractService, string ensRegistryAddress = "0x00000000000C2E074eC69A0dFb2997BA6C7d2e1e")
        {
            if (string.IsNullOrEmpty(ensRegistryAddress))
                throw new ArgumentException("ensRegistryAddress cannot be null", nameof(ensRegistryAddress));
            _ethApiContractService = ethApiContractService;

            EnsRegistryAddress = ensRegistryAddress;
            _ensUtil = new EnsUtil();
            TLS = "eth";
            TLSNameHash = _ensUtil.GetNameHash(TLS).HexToByteArray();
        }

        public string EnsRegistryAddress { get; }
        public string TLS { get; private set; }
        public byte[] TLSNameHash { get; private set; }
        public string TLSRegisterAddress { get; private set; }
        public string TLSResolverAddress { get; private set; }
        public string TLSControllerAddress { get; private set; }
        public ENSRegistryService ENSRegistryService { get; private set; }
        public PublicResolverService TLSResolverService { get; private set; }
        public ETHRegistrarControllerService TLSRegistrarControllerService { get; private set; }
        public int MinimunDurationRegistrationInSeconds { get; private set; }

        private readonly EnsUtil _ensUtil;

#if !DOTNET35
        public async UniTask InitialiseAsync()
        {
            ENSRegistryService = new ENSRegistryService(_ethApiContractService, EnsRegistryAddress);
            TLSRegisterAddress = await ENSRegistryService.OwnerQueryAsync(TLSNameHash);
            TLSResolverAddress = await ENSRegistryService.ResolverQueryAsync(TLSNameHash);
            TLSResolverService = new PublicResolverService(_ethApiContractService, TLSResolverAddress);
            TLSControllerAddress = await TLSResolverService.InterfaceImplementerQueryAsync(TLSNameHash, "0x018fac06".HexToByteArray());
            TLSRegistrarControllerService = new ETHRegistrarControllerService(_ethApiContractService, TLSControllerAddress);
            MinimunDurationRegistrationInSeconds = (int) await TLSRegistrarControllerService.MinRegistrationDurationQueryAsync();
        }

        public int GetMinimumDurationInDays()
        {
            return ConvertDurationToDays(MinimunDurationRegistrationInSeconds);
        }

        public async UniTask<BigInteger> CalculateRentPriceAsync(string name, int durationInDays)
        {
            var duration = ConvertDurationInSecondsValidatingMinimum(durationInDays);
            return await TLSRegistrarControllerService.RentPriceQueryAsync(name, duration);
        }

        public async UniTask<decimal> CalculateRentPriceInEtherAsync(string name, int durationInDays)
        {
            var rentPriceWei = await CalculateRentPriceAsync(name, durationInDays);
            return Util.UnitConversion.Convert.FromWei(rentPriceWei);
        }

        public int ConvertDurationToSeconds(int durationInDays)
        {
            return 60 * 60 * 24 * durationInDays;
        }

        public int ConvertDurationToDays(int durationInSeconds)
        {
            return durationInSeconds / (60 * 60 * 24);
        }

        public UniTask<bool> IsNameAvailableAsync(string name)
        {
            return TLSRegistrarControllerService.AvailableQueryAsync(name);
        }

        private int ConvertDurationInSecondsValidatingMinimum(int durationInDays)
        {
            var duration = ConvertDurationToSeconds(durationInDays);
            if (duration < MinimunDurationRegistrationInSeconds) throw new Exception($"Duration has to be bigger than the minimum duration: {GetMinimumDurationInDays()} days");
            return duration;
        }

        public byte[] ConvertSecretToHash(string secret)
        {
            return Util.Sha3Keccack.Current.CalculateHash(secret).HexToByteArray();
        }

        public UniTask<byte[]> CalculateCommitmentAsync(string name, string owner, string secret)
        {
            var fullSecret = ConvertSecretToHash(secret);
            return TLSRegistrarControllerService.MakeCommitmentQueryAsync(name, owner, fullSecret);
        }
    
        public async UniTask<string> CommitRequestAsync(string name, string owner, string secret)
        {
            var commitment = await CalculateCommitmentAsync(name, owner, secret);
            return await TLSRegistrarControllerService.CommitRequestAsync(commitment);
        }

        public async UniTask<TransactionReceipt> CommitRequestAndWaitForReceiptAsync(string name, string owner, string secret, CancellationToken cancellationToken = default)
        {
            var commitment = await CalculateCommitmentAsync(name, owner, secret);
            return await TLSRegistrarControllerService.CommitRequestAndWaitForReceiptAsync(commitment, cancellationToken);
        }

        public UniTask<string> RegisterRequestAsync(string name, string owner, int durationInDays, string secret, decimal etherRentPrice)
        {
            var weiPrice = Util.UnitConversion.Convert.ToWei(etherRentPrice);
            return RegisterRequestAsync(name, owner, durationInDays, secret, weiPrice);
        }


        public UniTask<string> RegisterRequestAsync(string name, string owner, int durationInDays, string secret, BigInteger weiRentPrice)
        {
            var registerFunction = CreateRegisterFunction(name, owner, durationInDays, secret, weiRentPrice);
            return TLSRegistrarControllerService.RegisterRequestAsync(registerFunction);
        }

        private RegisterFunction CreateRegisterFunction(string name, string owner, int durationInDays, string secret, BigInteger weiRentPrice)
        {
            var fullSecret = ConvertSecretToHash(secret);
            var duration = ConvertDurationInSecondsValidatingMinimum(durationInDays);
            var registerFunction = new RegisterFunction()
            {
                Name = name,
                AmountToSend = weiRentPrice,
                Duration = duration,
                Owner = owner,
                Secret = fullSecret
            };
            return registerFunction;
        }

        public UniTask<TransactionReceipt> RegisterRequestAndWaitForReceiptAsync(string name, string owner, int durationInDays, string secret, decimal etherRentPrice, CancellationToken cancellationToken = default)
        {
            var weiPrice = Util.UnitConversion.Convert.ToWei(etherRentPrice);
            return RegisterRequestAndWaitForReceiptAsync(name, owner, durationInDays, secret, weiPrice, cancellationToken);
        }

        public UniTask<TransactionReceipt> RegisterRequestAndWaitForReceiptAsync(string name, string owner, int durationInDays, string secret, BigInteger weiRentPrice, CancellationToken cancellationToken = default)
        {
            var registerFunction = CreateRegisterFunction(name, owner, durationInDays, secret, weiRentPrice);
            return TLSRegistrarControllerService.RegisterRequestAndWaitForReceiptAsync(registerFunction, cancellationToken);
        }

        public UniTask<string> CommitRequestAsync(byte[] commitment)
        {
            return TLSRegistrarControllerService.CommitRequestAsync(commitment);
        }

        public UniTask<TransactionReceipt> CommitRequestAndWaitForReceiptAsync(byte[] commitment, CancellationToken cancellationToken = default)
        {
            return TLSRegistrarControllerService.CommitRequestAndWaitForReceiptAsync(commitment, cancellationToken);
        }

        public UniTask<string> RenewRequestAsync(string name, int durationInDays, decimal etherRentPrice)
        {
            var weiPrice = Util.UnitConversion.Convert.ToWei(etherRentPrice);
            return RenewRequestAsync(name, durationInDays, weiPrice);
        }

        public UniTask<string> RenewRequestAsync(string name, int durationInDays, BigInteger weiRentPrice)
        {
            var renewFunction = CreateRenewFunction(name, durationInDays, weiRentPrice);
            return TLSRegistrarControllerService.RenewRequestAsync(renewFunction);
        }

        private RenewFunction CreateRenewFunction(string name, int durationInDays, BigInteger weiPrice)
        {
            var duration = ConvertDurationInSecondsValidatingMinimum(durationInDays);
            var renewFunction = new RenewFunction()
            {
                AmountToSend = weiPrice,
                Duration = duration,
                Name = name
            };
            return renewFunction;
        }
#endif
    }
}
