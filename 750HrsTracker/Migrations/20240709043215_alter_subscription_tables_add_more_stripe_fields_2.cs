using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_subscription_tables_add_more_stripe_fields_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventDataObject",
                table: "SubscriptionTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeEventId",
                table: "SubscriptionTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeEventName",
                table: "SubscriptionTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDataObject",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "StripeEventId",
                table: "SubscriptionTransactions");

            migrationBuilder.DropColumn(
                name: "StripeEventName",
                table: "SubscriptionTransactions");
        }
    }
}
