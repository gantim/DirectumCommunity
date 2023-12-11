using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class AddLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NeedChangePassword = table.Column<bool>(type: "boolean", nullable: true),
                    LoginName = table.Column<string>(type: "text", nullable: true),
                    TypeAuthentication = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    PasswordLastChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LoginId",
                table: "Employees",
                column: "LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Login_LoginId",
                table: "Employees",
                column: "LoginId",
                principalTable: "Login",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Login_LoginId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LoginId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "Employees");
        }
    }
}
