﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BidLists",
                columns: table => new
                {
                    BidListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidQuantity = table.Column<double>(type: "float", nullable: true),
                    Ask = table.Column<double>(type: "float", nullable: true),
                    Benchmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidListDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Commentary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidSecurity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Side = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidLists", x => x.BidListId);
                });

            migrationBuilder.CreateTable(
                name: "CurvePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurveId = table.Column<byte>(type: "tinyint", nullable: true),
                    AsOfDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<double>(type: "float", nullable: true),
                    CurvePointValue = table.Column<double>(type: "float", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurvePoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoodyRating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SandPRating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FitchRating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNumber = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SqlStr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SqlPart = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    TradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyQuantity = table.Column<double>(type: "float", nullable: true),
                    SellQuantity = table.Column<double>(type: "float", nullable: true),
                    BuyPrice = table.Column<double>(type: "float", nullable: true),
                    SellPrice = table.Column<double>(type: "float", nullable: true),
                    TradeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TradeSecurity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TradeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Benchmark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceListId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Side = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidLists");

            migrationBuilder.DropTable(
                name: "CurvePoints");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "RuleNames");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
