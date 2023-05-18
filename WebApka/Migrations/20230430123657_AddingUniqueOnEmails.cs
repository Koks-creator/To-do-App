﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApka.Migrations
{
    /// <inheritdoc />
    public partial class AddingUniqueOnEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Unique",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email_Unique",
                table: "Users");
        }
    }
}
