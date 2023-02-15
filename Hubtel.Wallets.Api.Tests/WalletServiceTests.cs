using AutoMapper;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Helpers;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hubtel.Wallets.Api.Tests
{
    public class WalletServiceTests
    {
        Wallet testWallet;
        WalletDto walletDto;
        List<Wallet> allWallets;
        IMapper mapper;

        public WalletServiceTests()
        {
            allWallets = new List<Wallet>
            {
                new Wallet
                {
                    ID = 1,
                    Name = "test",
                    AccountNumber = "0000",
                    CreatedAt = DateTime.Now,
                    Owner = "0000000000",
                    AccountScheme = new AccountScheme { ID = 1, Scheme = "testscheme" },
                    AccountType = new AccountType { ID = 1, Type = "testtype" }
                },
                new Wallet
                {
                    ID = 2,
                    Name = "test2",
                    AccountNumber = "0000",
                    CreatedAt = DateTime.Now,
                    Owner = "0000000000",
                    AccountScheme = new AccountScheme { ID = 1, Scheme = "testscheme" },
                    AccountType = new AccountType { ID = 1, Type = "testtype" }
                }, 
                new Wallet
                {
                    ID = 3,
                    Name = "test3",
                    AccountNumber = "0000",
                    CreatedAt = DateTime.Now,
                    Owner = "0000000000",
                    AccountScheme = new AccountScheme { ID = 1, Scheme = "testscheme" },
                    AccountType = new AccountType { ID = 1, Type = "testtype" }
                }
            };

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

            walletDto = new WalletDto
            {
                Name= "test wallet",
                Type = "type",
                AccountNumber = "0000000000000000",
                AccountScheme = "scheme",
                Owner = "0000000000"
            };

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public void GetByIdReturnsData()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetWalletById(1)).Returns(testWallet);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var wallet = walletService.GetWalletById(1);

            // Assert
            Assert.Equal(testWallet.Owner, wallet.Owner); 
        }

        [Fact]
        public void AddReturnsErrorIfAccountAlreadyExists()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.WalletAlreadyExists(It.IsAny<string>())).Returns(true);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var error = walletService.AddWallet(walletDto);

            // Assert
            Assert.Equal("Wallet already exists", error);
        }

        [Fact]
        public void AddReturnsErrorIfWalletCountExceeded()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.WalletCountExceeded(It.IsAny<string>())).Returns(true);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var error = walletService.AddWallet(walletDto);

            // Assert
            Assert.Equal("Wallet count limit exceeded", error);
        }

        [Fact]
        public void AddReturnsErrorIfSchemeNotFound()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(() => null);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var error = walletService.AddWallet(walletDto);

            // Assert
            Assert.Equal("Scheme does not exist", error);
        }

        [Fact]
        public void AddReturnsErrorIfTypeNotFound()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(new AccountScheme { ID = 1, Scheme = "scheme"});
            mock.Setup(w => w.GetType(It.IsAny<string>())).Returns(() => null);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var error = walletService.AddWallet(walletDto);

            // Assert
            Assert.Equal("Type does not exist", error);
        }

        [Fact]
        public void AddTrimsNumberIfVisaCard()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(new AccountScheme { ID = 1, Scheme = "visa" });
            mock.Setup(w => w.GetType(It.IsAny<string>())).Returns(new AccountType { ID = 1, Type = "card"});
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            walletService.AddWallet(walletDto);

            // Assert
            Assert.Equal("000000", walletDto.AccountNumber);
        }

        [Fact]
        public void AddTrimsNumberIfMasterCard()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(new AccountScheme { ID = 1, Scheme = "mastercard" });
            mock.Setup(w => w.GetType(It.IsAny<string>())).Returns(new AccountType { ID = 1, Type = "card" });
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            walletService.AddWallet(walletDto);

            // Assert
            Assert.Equal("000000", walletDto.AccountNumber);
        }

        [Fact]
        public void GetAllWalletsReturnsData()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetAllWallets()).Returns(allWallets);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var wallets = walletService.GetAllWallets();

            // Assert
            Assert.Equal(3, wallets.Count);
        }

        [Fact]
        public void DeleteWallet()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetWalletById(It.IsAny<int>())).Returns(testWallet);
            mock.Setup(w => w.DeleteWallet(It.IsAny<Wallet>())).Verifiable();
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var deleted = walletService.DeleteWallet(testWallet.ID);

            // Assert
            mock.Verify(m => m.DeleteWallet(It.IsAny<Wallet>()), Times.Once());
            Assert.True(deleted);
        }
    }
}
