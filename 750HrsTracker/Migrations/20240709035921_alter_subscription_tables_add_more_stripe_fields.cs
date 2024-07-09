using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_subscription_tables_add_more_stripe_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionTransactionId",
                table: "TeamSubscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckoutTransaction",
                table: "SubscriptionTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "SubscriptionTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionTransactionId",
                table: "TeamSubscriptions");

            migrationBuilder.DropColumn(
                name: "IsCheckoutTransaction",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "SubscriptionTransactions");
        }
    }
}
