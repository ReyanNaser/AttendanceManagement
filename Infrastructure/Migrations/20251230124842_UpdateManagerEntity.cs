using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManagerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_manager_employee_UserDetailsId",
                table: "manager");

            migrationBuilder.DropIndex(
                name: "IX_manager_UserDetailsId",
                table: "manager");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "manager");

            migrationBuilder.AddColumn<string>(
                name: "designation",
                table: "manager",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "manager",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "manager",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "designation",
                table: "manager");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "manager");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "manager");

            migrationBuilder.AddColumn<Guid>(
                name: "UserDetailsId",
                table: "manager",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_manager_UserDetailsId",
                table: "manager",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_manager_employee_UserDetailsId",
                table: "manager",
                column: "UserDetailsId",
                principalTable: "employee",
                principalColumn: "id");
        }
    }
}
