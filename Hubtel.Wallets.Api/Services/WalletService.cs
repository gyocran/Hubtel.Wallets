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

        public int AddWallet(WalletDto wallet)
        {
            if (AccountIsCard(wallet))
                Utilities.TrimCardNumber(wallet);

            var newWallet = ConvertDtoToWalletEntity(wallet);

            _repo.AddWallet(newWallet);

            return newWallet.ID;
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

        private bool AccountIsCard(WalletDto wallet)
        {
            return wallet.Type.ToLower() == "card" && (wallet.AccountScheme.ToLower() == "visa" || wallet.AccountScheme.ToLower() == "mastercard");
        }

        private Wallet ConvertDtoToWalletEntity(WalletDto walletDto)
        {
            var scheme = GetScheme(walletDto);
            var type = GetType(walletDto);

            // populate fields for new wallet
            var newWallet = _mapper.Map<Wallet>(walletDto);
            newWallet.AccountScheme = scheme;
            newWallet.AccountType = type;
            newWallet.CreatedAt = DateTime.Now;

            return newWallet;
        }

        public bool WalletAlreadyExists(WalletDto wallet)
        {
            if (_repo.WalletAlreadyExists(wallet.AccountNumber))
                return true;

            return false;
        }
        
        public bool WalletCountExceeded(WalletDto wallet)
        {
            if (_repo.WalletCountExceeded(wallet.Owner))
                return true;

            return false;
        }

        public AccountScheme GetScheme(WalletDto wallet)
        {
            return _repo.GetScheme(wallet.AccountScheme);
        }

        public AccountType GetType(WalletDto wallet)
        {
            return _repo.GetType(wallet.Type);
        }

        public bool AccountNumberLengthIsInvalid(WalletDto wallet)
        {
            Utilities.RemoveWhiteSpaces(wallet.AccountNumber);

            if (AccountIsCard(wallet))
                return wallet.AccountNumber.Length > 16;
            else
                return wallet.AccountNumber.Length > 10;
        }

        public bool OwnerLengthIsInvalid(WalletDto wallet)
        {
            Utilities.RemoveWhiteSpaces(wallet.Owner);

            return wallet.Owner.Length > 10;
        }

        public bool SchemeDoesNotExist(WalletDto wallet)
        {
            return _repo.SchemeDoesNotExist(wallet.AccountScheme);
        }

        public bool TypeDoesNotExist(WalletDto wallet)
        {
            return _repo.TypeDoesNotExist(wallet.Type);
        }

        public bool AccountNumberContainsNonNumeric(WalletDto wallet)
        {
            return Utilities.ContainsNonNumericCharacters(wallet.AccountNumber);
        }

        public bool OwnerContainsNonNumeric(WalletDto wallet)
        {
            return Utilities.ContainsNonNumericCharacters(wallet.Owner);
        }

        public string ReturnWalletError(WalletDto wallet)
        {
            if (AccountNumberContainsNonNumeric(wallet))
                return "Account number contains non numeric characters";

            if (OwnerContainsNonNumeric(wallet))
                return "Owner contains non numeric characters";

            if (WalletAlreadyExists(wallet))
                return "Wallet already exists";

            if (WalletCountExceeded(wallet))
                return "Wallet count limit exceeded";

            if (SchemeDoesNotExist(wallet))
                return "Scheme does not exist";

            if (TypeDoesNotExist(wallet))
                return "Type does not exist";

            if (AccountNumberLengthIsInvalid(wallet))
                return "Account number length invalid";

            if (OwnerLengthIsInvalid(wallet))
                return "Owner length invalid";

            return string.Empty;
        }
    }
}
