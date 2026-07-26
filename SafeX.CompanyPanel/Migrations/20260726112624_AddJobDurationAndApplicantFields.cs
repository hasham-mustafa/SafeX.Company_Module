using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeX.CompanyPanel.Migrations
{
    /// <inheritdoc />
    public partial class AddJobDurationAndApplicantFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Jobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BidAmount",
                table: "Applicants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Applicants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proposal",
                table: "Applicants",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "Applicants",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Applicants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "BidAmount",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Proposal",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "University",
                table: "Applicants");
        }
    }
}
