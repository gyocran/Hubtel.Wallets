using AutoMapper;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Helpers;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService : IWalletService
    {
        private IMapper _mapper;
        private IWalletRepo _repo;

        public WalletService(IWalletRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public string AddWallet(WalletDto wallet)
        {
            if (_repo.WalletAlreadyExists(wallet.AccountNumber))
                return "Wallet already exists";

            if (_repo.WalletCountExceeded(wallet.Owner))
                return "Wallet count limit exceeded";

            // get scheme and type from db
            var dbScheme = _repo.GetScheme(wallet.AccountScheme);
            var dbType = _repo.GetType(wallet.Type);

            if (dbScheme == null)
                return "Scheme does not exist";

            if (dbType == null)
                return "Type does not exist";

            if (AccountIsCard(dbType.Type.ToLower(), dbScheme.Scheme.ToLower()))
            {
                wallet.AccountNumber = Utilities.TrimCardNumber(wallet.AccountNumber);
            }

            var newWallet = ConvertDtoToWalletEntity(wallet, dbType, dbScheme);

            _repo.AddWallet(newWallet);
            return null;
        }

        public List<WalletDto> GetAllWallets()
        {
            return _mapper.Map<List<WalletDto>>(_repo.GetAllWallets());
        }

        public WalletDto GetWalletById(int Id)
        {
            return _mapper.Map<WalletDto>(_repo.GetWalletById(Id));
        }

        public bool DeleteWallet(int Id)
        {
            var wallet = _repo.GetWalletById(Id);
            if (wallet != null)
            {
                _repo.DeleteWallet(wallet);
                return true;
            }

            return false;
        }

        private bool AccountIsCard(string type, string scheme)
        {
            return type == "card" && (scheme == "visa" || scheme == "mastercard");
        }

        private Wallet ConvertDtoToWalletEntity(WalletDto walletDto, AccountType type, AccountScheme scheme)
        {
            // populate fields for new wallet
            var newWallet = _mapper.Map<Wallet>(walletDto);
            newWallet.AccountScheme = scheme;
            newWallet.AccountType = type;
            newWallet.CreatedAt = DateTime.Now;

            return newWallet;
        }
    }
}
