using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class updated_team_subscription_key_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions");

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionTransactionId",
                table: "TeamSubscriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "TeamSubscriptions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionId",
                table: "TeamSubscriptions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeamSubscriptions_SubscriptionId",
                table: "TeamSubscriptions",
                column: "SubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_TeamSubscriptions_SubscriptionId",
                table: "TeamSubscriptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "TeamSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionTransactionId",
                table: "TeamSubscriptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionId",
                table: "TeamSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamSubscriptions",
                table: "TeamSubscriptions",
                columns: new[] { "SubscriptionId", "TeamId", "SubscriptionTransactionId" });
        }
    }
}
