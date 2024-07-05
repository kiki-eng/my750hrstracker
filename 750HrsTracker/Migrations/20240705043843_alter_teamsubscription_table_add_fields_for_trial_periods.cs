using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _750HrsTracker.Migrations
{
    public partial class alter_teamsubscription_table_add_fields_for_trial_periods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StillOnTrial",
                table: "TeamSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrialEndDate",
                table: "TeamSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrialStartDate",
                table: "TeamSubscriptions",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StillOnTrial",
                table: "TeamSubscriptions");

            migrationBuilder.DropColumn(
                name: "TrialEndDate",
                table: "TeamSubscriptions");

            migrationBuilder.DropColumn(
                name: "TrialStartDate",
                table: "TeamSubscriptions");
        }
    }
}
