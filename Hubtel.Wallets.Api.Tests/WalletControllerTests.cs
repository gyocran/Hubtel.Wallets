using AutoMapper;
using Hubtel.Wallets.Api.Controllers;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Interfaces;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Hubtel.Wallets.Api.Tests
{
    public class WalletControllerTests
    {
        Wallet testWallet;
        WalletDto testWalletDto;
        List<WalletDto> testWalletDtoList;

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

            testWalletDto = new WalletDto
            {
                ID = 1,
                Name = "test wallet",
                Type = "type",
                AccountNumber = "0000000000000000",
                AccountScheme = "scheme",
                Owner = "0000000000"
            };

            testWalletDtoList = new List<WalletDto>
            {
                testWalletDto,
                new WalletDto
                {
                    ID = 2,
                    Name = "test wallet 2",
                    Type = "type",
                    AccountNumber = "0000000000000002",
                    AccountScheme = "scheme",
                    Owner = "00000000002"
                },
                new WalletDto
                {
                    ID = 3,
                    Name = "test wallet 3",
                    Type = "type",
                    AccountNumber = "0000000000000003",
                    AccountScheme = "scheme",
                    Owner = "00000000003"
                },
            };
        }

        [Fact]
        public void GetReturnsWallet()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.GetWalletById(It.IsAny<int>())).Returns(testWalletDto);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Get(It.IsAny<int>());

            // Assert
            var okResult = result as OkObjectResult;
            var wallet = okResult.Value as WalletDto;
            Assert.NotNull(wallet);
            Assert.Equal(1, wallet.ID);
        }

        [Fact]
        public void GetAllReturnsWallets()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.GetAllWallets()).Returns(testWalletDtoList);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            var wallets = okResult.Value as List<WalletDto>;
            Assert.NotNull(wallets);
            Assert.Equal(3, wallets.Count);
        }

        [Fact]
        public void CreateWalletReturnsIdIfCreated()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.AddWallet(It.IsAny<WalletDto>())).Returns(5);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Post(testWalletDto);

            // Assert
            var okResult = result as OkObjectResult;
            var id = okResult.Value;
            Assert.Equal(5, id);
        }

        [Fact]
        public void CreateWalletReturnsBadRequestIfNullPassed()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Post(null);

            // Assert
            var badResult = result as BadRequestObjectResult;
            Assert.IsType<SerializableError>(badResult.Value);
        }

        [Fact]
        public void CreateWalletReturnsBadRequestIfErrorFound()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.ReturnWalletError(testWalletDto)).Returns("Wallet already exists");
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Post(testWalletDto);

            // Assert
            var badResult = result as BadRequestObjectResult;
            var message = badResult.Value;
            Assert.Equal("Wallet already exists", message);
        }

        [Fact]
        public void DeleteWalletReturnsNotFoundIfIdInvalid()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.DeleteWallet(It.IsAny<int>())).Returns(true);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Delete(0);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.IsType<NotFoundResult>(objectResult);
        }

        [Fact]
        public void DeleteWalletReturnsNotFoundIfIdNotInDb()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.DeleteWallet(It.IsAny<int>())).Returns(false);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Delete(4);

            // Assert
            var objectResult = result as NotFoundResult;
            Assert.IsType<NotFoundResult>(objectResult);
        }

        [Fact]
        public void DeleteWalletReturnsAcceptedIfDeleted()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.DeleteWallet(It.IsAny<int>())).Returns(true);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Delete(4);

            // Assert
            var objectResult = result as AcceptedResult;
            Assert.IsType<AcceptedResult>(objectResult);
        }
    }
}
