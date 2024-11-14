using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_tables_for_subscriptions_in_app_purchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMobileInAppPurchase",
                table: "SubscriptionTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "SubscriptionTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "SubscriptionTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AndroidProductId",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IosProductId",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMobileInAppPurchase",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "AndroidProductId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IosProductId",
                table: "Subscriptions");
        }
    }
}
