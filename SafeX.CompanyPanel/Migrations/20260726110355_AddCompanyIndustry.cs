using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeX.CompanyPanel.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIndustry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Companies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Companies");
        }
    }
}
