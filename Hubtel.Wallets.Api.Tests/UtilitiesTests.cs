using Hubtel.Wallets.Api.Controllers;
using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Helpers;
using Hubtel.Wallets.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Hubtel.Wallets.Api.Tests
{
    public class UtilitiesTests
    {
        IUtilities utils = new Utilities();

        [Fact]
        public void TrimsCardNumber()
        {
            // Arrange
            WalletDto wallet = new WalletDto
            {
                AccountNumber = "0001112223334444"
            };

            // Act
            utils.TrimCardNumber(wallet);

            // Assert
            Assert.Equal("000111", wallet.AccountNumber);
        }

        [Fact]
        public void RemovesWhiteSpaces()
        {
            // Arrange
            var number = "111 3233 3324";

            // Act
            utils.RemoveWhiteSpaces(number);

            // Assert
            Assert.Equal("11132333324", number);
        }
    }
}
