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
                Type = "card",
                AccountNumber = "0000000000000000",
                AccountScheme = "visa",
                Owner = "0000000000"
            };

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public void AddWalletReturnsWalletId()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.AddWallet(testWallet)).Callback(() => testWallet.ID = 3);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var walletId = walletService.AddWallet(walletDto);

            // Assert
            //mock.Verify(m => m.AddWallet(testWallet), Times.Once());
            Assert.Equal(3, walletId);
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
        public void ReturnsTrueIfAccountAlreadyExists()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.WalletAlreadyExists(It.IsAny<string>())).Returns(true);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var exists = walletService.WalletAlreadyExists(walletDto);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void ReturnsErrorIfWalletCountExceeded()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.WalletCountExceeded(It.IsAny<string>())).Returns(true);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var exceeded = walletService.WalletCountExceeded(walletDto);

            // Assert
            Assert.True(exceeded);
        }

        [Fact]
        public void ReturnsTrueIfSchemeNotFound()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.SchemeDoesNotExist(It.IsAny<string>())).Returns(true);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var notExists = walletService.SchemeDoesNotExist(walletDto);

            // Assert
            Assert.True(notExists);
        }

        [Fact]
        public void ReturnsTrueIfTypeNotFound()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.TypeDoesNotExist(It.IsAny<string>())).Returns(true);
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var notExists = walletService.TypeDoesNotExist(walletDto);

            // Assert
            Assert.True(notExists);
        }

        //[Fact]
        //public void AddTrimsNumberIfVisaCard()
        //{
        //    // Arrange
        //    var mock = new Mock<IWalletRepo>();
        //    mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(new AccountScheme { ID = 1, Scheme = "visa" });
        //    mock.Setup(w => w.GetType(It.IsAny<string>())).Returns(new AccountType { ID = 1, Type = "card"});
        //    var walletService = new WalletService(mock.Object, mapper);

        //    // Act
        //    walletService.AddWallet(walletDto);

        //    // Assert
        //    Assert.Equal("000000", walletDto.AccountNumber);
        //}

        //[Fact]
        //public void AddTrimsNumberIfMasterCard()
        //{
        //    // Arrange
        //    var mock = new Mock<IWalletRepo>();
        //    mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(new AccountScheme { ID = 1, Scheme = "mastercard" });
        //    mock.Setup(w => w.GetType(It.IsAny<string>())).Returns(new AccountType { ID = 1, Type = "card" });
        //    var walletService = new WalletService(mock.Object, mapper);

        //    // Act
        //    walletService.AddWallet(walletDto);

        //    // Assert
        //    Assert.Equal("000000", walletDto.AccountNumber);
        //}

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

        [Fact]
        public void GetSchemeReturnsScheme()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetScheme(It.IsAny<string>())).Returns(new AccountScheme { ID = 1, Scheme = "visa" });
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var scheme = walletService.GetScheme(walletDto);

            // Assert
            Assert.Equal(1, scheme.ID);
            Assert.Equal("visa", scheme.Scheme);
        }

        [Fact]
        public void GetTypeReturnsType()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            mock.Setup(w => w.GetType(It.IsAny<string>())).Returns(new AccountType { ID = 1, Type = "momo" });
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var type = walletService.GetType(walletDto);

            // Assert
            Assert.Equal(1, type.ID);
            Assert.Equal("momo", type.Type);
        }

        [Fact]
        public void ReturnsTrueIfCardAccountNumberLengthIsInvalid()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            walletDto.AccountNumber = "24334234324343242342432432432";
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var invalid = walletService.AccountNumberLengthIsInvalid(walletDto);

            // Assert
            Assert.True(invalid);
        }

        [Fact]
        public void ReturnsTrueIfOwnerLengthIsInvalid()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            walletDto.Owner = "345433433454354345";
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var invalid = walletService.OwnerLengthIsInvalid(walletDto);

            // Assert
            Assert.True(invalid);
        }

        [Fact]
        public void ReturnsTrueIfMomoAccountNumberLengthIsInvalid()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            walletDto.Type = "card";
            walletDto.AccountScheme = "mtn";
            walletDto.AccountNumber = "3243252352423";
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var invalid = walletService.AccountNumberLengthIsInvalid(walletDto);

            // Assert
            Assert.True(invalid);
        }

        [Fact]
        public void ReturnsTrueIfAccountNumberContainsNonNumeric()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            walletDto.AccountNumber = "dsafds3243252352423";
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var invalid = walletService.AccountNumberContainsNonNumeric(walletDto);

            // Assert
            Assert.True(invalid);
        }

        [Fact]
        public void ReturnsTrueIfOwnerContainsNonNumeric()
        {
            // Arrange
            var mock = new Mock<IWalletRepo>();
            walletDto.Owner = "dsafd352423";
            var walletService = new WalletService(mock.Object, mapper);

            // Act
            var invalid = walletService.OwnerContainsNonNumeric(walletDto);

            // Assert
            Assert.True(invalid);
        }
    }
}
