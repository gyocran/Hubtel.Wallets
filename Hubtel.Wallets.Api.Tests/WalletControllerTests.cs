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
        public void CreateWallet()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.AddWallet(It.IsAny<WalletDto>())).Returns("Wallet already exists");
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Post(It.IsAny<WalletDto>());

            // Assert
            var okResult = result as OkObjectResult;
            var message = okResult.Value;
            Assert.Equal("wallet created", message);
        }

        [Fact]
        public void DeleteWallet()
        {
            // Arrange
            var mock = new Mock<IWalletService>();
            mock.Setup(w => w.DeleteWallet(It.IsAny<int>())).Returns(true);
            var walletController = new WalletController(mock.Object);

            // Act
            var result = walletController.Delete(It.IsAny<int>());

            // Assert
            var acceptedResult = result as AcceptedAtActionResult;
            //var actionResult = Assert.IsType<ActionResult<List<IdeaDTO>>>(result);
            Assert.IsType<AcceptedAtActionResult>(acceptedResult);
        }
    }
}
