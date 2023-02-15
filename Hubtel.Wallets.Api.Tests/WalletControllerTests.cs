using AutoMapper;
using Hubtel.Wallets.Api.Controllers;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Hubtel.Wallets.Api.Tests
{
    public class WalletControllerTests
    {
        Wallet testWallet;

        public WalletControllerTests() 
        {
            testWallet = new Wallet
            {
                ID = 1,
                Name = "test",
                AccountNumber = "0000",
                CreatedAt = DateTime.Now,
                Owner = "0000000000",
                AccountScheme = new AccountScheme { ID = 1, Scheme = "testscheme" },
                AccountType = new AccountType { ID = 1, Type = "testtype" }
            };
        }

        [Fact]
        public void GetReturnsWallet()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.GetWalletById(It.IsAny<int>())).Returns(testWallet);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Get(It.IsAny<int>());

            // Assert
            Assert.Equal("Wallet already exists", error);
        }
    }
}
