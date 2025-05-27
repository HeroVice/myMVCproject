﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMVCProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCoverImageUrlToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Products");
        }
    }
}
