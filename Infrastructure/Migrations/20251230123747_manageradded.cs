using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class manageradded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "manager_id",
                table: "employee",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "manager",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserDetailsId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manager", x => x.id);
                    table.ForeignKey(
                        name: "FK_manager_employee_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "employee",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_manager_id",
                table: "employee",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "IX_manager_UserDetailsId",
                table: "manager",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_manager_manager_id",
                table: "employee",
                column: "manager_id",
                principalTable: "manager",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_manager_manager_id",
                table: "employee");

            migrationBuilder.DropTable(
                name: "manager");

            migrationBuilder.DropIndex(
                name: "IX_employee_manager_id",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "manager_id",
                table: "employee");
        }
    }
}
