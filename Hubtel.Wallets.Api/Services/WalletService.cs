using AutoMapper;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Helpers;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService : IWalletService
    {
        private IMapper _mapper;
        private IWalletRepo _repo;
        private IUtilities _utils;

        public WalletService(IWalletRepo repo, IMapper mapper, IUtilities utils)
        {
            _repo = repo;
            _mapper = mapper;
            _utils = utils;
        }

        public int AddWallet(WalletDto wallet)
        {
            if (AccountIsCard(wallet))
                _utils.TrimCardNumber(wallet);

            var newWallet = ConvertDtoToWalletEntity(wallet);

            _repo.AddWallet(newWallet);

            return newWallet.ID;
        }

        public List<WalletDto> GetAllWallets(AllWalletsParameters queryParams)
        {
            var allWallets = _mapper.Map<List<WalletDto>>(_repo.GetAllWallets());
            return allWallets
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();
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

        public bool AccountIsCard(WalletDto wallet)
        {
            return wallet.Type.ToLower() == "card" && 
                (wallet.AccountScheme.ToLower() == "visa" || wallet.AccountScheme.ToLower() == "mastercard");
        }

        public Wallet ConvertDtoToWalletEntity(WalletDto walletDto)
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

        public bool WalletAlreadyExists(WalletDto walletDto)
        {
            var wallet = _repo.GetWalletByAccountNumber(walletDto.AccountNumber);

            // return true if exists
            return wallet != null;
        }
        
        public bool WalletCountExceeded(WalletDto wallet)
        {
            var ownerWalletCount = _repo.GetOwnerWalletCount(wallet.Owner);
            var walletCountLimit = _repo.GetWalletCountLimit();

            // return true if limit exceeded
            return ownerWalletCount >= walletCountLimit;
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
            _utils.RemoveWhiteSpaces(wallet.AccountNumber);

            if (AccountIsCard(wallet))
                return wallet.AccountNumber.Length != 16;
            else
                return wallet.AccountNumber.Length != 10;
        }

        public bool OwnerLengthIsInvalid(WalletDto wallet)
        {
            _utils.RemoveWhiteSpaces(wallet.Owner);

            return wallet.Owner.Length > 10;
        }

        public bool SchemeDoesNotExist(WalletDto wallet)
        {
            var scheme = _repo.GetScheme(wallet.AccountScheme);
            // return true if does not exist
            return scheme == null;
        }

        public bool TypeDoesNotExist(WalletDto wallet)
        {
            var type = _repo.GetType(wallet.Type);
            // return true if does not exist
            return type == null;
        }

        public bool AccountNumberContainsNonNumeric(WalletDto wallet)
        {
            return _utils.ContainsNonNumericCharacters(wallet.AccountNumber);
        }

        public bool OwnerContainsNonNumeric(WalletDto wallet)
        {
            return _utils.ContainsNonNumericCharacters(wallet.Owner);
        }

        public string ReturnWalletError(WalletDto wallet)
        {
            if (AccountNumberContainsNonNumeric(wallet))
                return "Account number contains non numeric characters";

            if (OwnerContainsNonNumeric(wallet))
                return "Owner contains non numeric characters";

            if (AccountNumberLengthIsInvalid(wallet))
                return "Account number length invalid";

            if (OwnerLengthIsInvalid(wallet))
                return "Owner length invalid";

            if (SchemeDoesNotExist(wallet))
                return "Scheme does not exist";

            if (TypeDoesNotExist(wallet))
                return "Type does not exist";

            if (TypeSchemeMismatch(wallet))
                return "Type scheme mismatch";

            if (WalletAlreadyExists(wallet))
                return "Wallet already exists";

            if (WalletCountExceeded(wallet))
                return "Wallet count limit exceeded";                    

            return string.Empty;
        }

        public bool TypeSchemeMismatch(WalletDto wallet)
        {
            var type = wallet.Type.ToLower();
            List<AccountScheme> schemes;

            if (type == "momo")
                schemes = _repo.GetMomoSchemes();
            else
                schemes = _repo.GetCardSchemes();

            return !schemes.Exists(s => s.Scheme.ToLower() == wallet.AccountScheme.ToLower());
        }
    }
}
