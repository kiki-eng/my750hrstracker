using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class add_and_alter_subscription_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionInterval",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubscriptionTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamSubscriptionSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamSubscriptionTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InitialStripeSessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastActionById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionTransactions_TeamSubscriptions_TeamSubscriptionSubscriptionId_TeamSubscriptionTeamId",
                        columns: x => new { x.TeamSubscriptionSubscriptionId, x.TeamSubscriptionTeamId },
                        principalTable: "TeamSubscriptions",
                        principalColumns: new[] { "SubscriptionId", "TeamId" });
                    table.ForeignKey(
                        name: "FK_SubscriptionTransactions_Users_LastActionById",
                        column: x => x.LastActionById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactions_LastActionById",
                table: "SubscriptionTransactions",
                column: "LastActionById");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactions_TeamSubscriptionSubscriptionId_TeamSubscriptionTeamId",
                table: "SubscriptionTransactions",
                columns: new[] { "TeamSubscriptionSubscriptionId", "TeamSubscriptionTeamId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "SubscriptionInterval",
                table: "Subscriptions");
        }
    }
}
