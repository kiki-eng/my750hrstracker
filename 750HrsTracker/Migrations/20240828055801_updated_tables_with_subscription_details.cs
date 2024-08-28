using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class updated_tables_with_subscription_details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Canceled",
                table: "TeamSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledAt",
                table: "TeamSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "TeamSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsedTrial",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "TeamSubscriptions");

            migrationBuilder.DropColumn(
                name: "CanceledAt",
                table: "TeamSubscriptions");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "TeamSubscriptions");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "UsedTrial",
                table: "Teams");
        }
    }
}
