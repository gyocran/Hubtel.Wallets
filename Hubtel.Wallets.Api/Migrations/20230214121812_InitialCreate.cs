using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hubtel.Wallets.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountScheme",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scheme = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountScheme", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WalletLimit",
                columns: table => new
                {
                    WalletCountLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 16, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Owner = table.Column<string>(maxLength: 10, nullable: false),
                    AccountSchemeID = table.Column<int>(nullable: true),
                    AccountTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Wallet_AccountScheme_AccountSchemeID",
                        column: x => x.AccountSchemeID,
                        principalTable: "AccountScheme",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wallet_AccountType_AccountTypeID",
                        column: x => x.AccountTypeID,
                        principalTable: "AccountType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_AccountSchemeID",
                table: "Wallet",
                column: "AccountSchemeID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_AccountTypeID",
                table: "Wallet",
                column: "AccountTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "WalletLimit");

            migrationBuilder.DropTable(
                name: "AccountScheme");

            migrationBuilder.DropTable(
                name: "AccountType");
        }
    }
}
