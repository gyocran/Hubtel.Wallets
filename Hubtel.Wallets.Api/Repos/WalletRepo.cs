using AutoMapper;
using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.Wallets.Api.Repos
{
    public class WalletRepo : IWalletRepo
    {
        private HubtelWalletDBContext _context;

        public WalletRepo(HubtelWalletDBContext context)
        {
            _context = context;
        }

        public void AddWallet(Wallet wallet)
        {
            _context.Wallet.Add(wallet);
            save();
        }

        public List<Wallet> GetAllWallets()
        {
            return _context.Wallet
                .Include(w => w.AccountScheme)
                .Include(w => w.AccountType)
                .ToList();
        }

        public Wallet GetWalletById(int Id)
        {
            //var wallet = _context.Wallet.FirstOrDefault(w => w.Id == Id);
            return _context.Wallet
                .Include(w => w.AccountScheme)
                .Include(w => w.AccountType)
                .Where(w => w.ID == Id)
                .FirstOrDefault();
        }

        public void DeleteWallet(Wallet wallet)
        {
            _context.Wallet.Remove(wallet);
            save();
        }

        public bool WalletAlreadyExists(string accountNumber)
        {
            var wallet = _context.Wallet.FirstOrDefault(w => w.AccountNumber == accountNumber);

            // return true if wallet exists;
            return wallet != null;
        }

        public bool WalletCountExceeded(string phoneNumber)
        {
            var count = _context.Wallet.ToList().Where(w => w.Owner == phoneNumber).Count();
            var limit = WalletCountLimit();
            // return true if limit exceeded
            return count > limit;
        }

        void save()
        {
            _context.SaveChanges();
        }

        public int WalletCountLimit()
        {
            return _context.WalletLimit.FirstOrDefault().WalletCountLimit;
        }

        public AccountScheme GetScheme(string schemeName)
        {
            return _context.AccountScheme.FirstOrDefault(s => s.Scheme == schemeName);
        }

        public AccountType GetType(string typeName)
        {
            return _context.AccountType.FirstOrDefault(s => s.Type == typeName);
        }
    }
}
