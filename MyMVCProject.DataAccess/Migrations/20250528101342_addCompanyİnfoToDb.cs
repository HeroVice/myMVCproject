using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMVCProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyİnfoToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CompanyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompAddress",
                table: "CompanyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "CompanyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CompanyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IDN",
                table: "CompanyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "CompanyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "CompanyRequests");

            migrationBuilder.DropColumn(
                name: "CompAddress",
                table: "CompanyRequests");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "CompanyRequests");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CompanyRequests");

            migrationBuilder.DropColumn(
                name: "IDN",
                table: "CompanyRequests");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "CompanyRequests");
        }
    }
}
