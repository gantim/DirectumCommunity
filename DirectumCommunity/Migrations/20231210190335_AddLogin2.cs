using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class AddLogin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Login_LoginId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Login",
                table: "Login");

            migrationBuilder.RenameTable(
                name: "Login",
                newName: "Logins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logins",
                table: "Logins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Logins_LoginId",
                table: "Employees",
                column: "LoginId",
                principalTable: "Logins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Logins_LoginId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logins",
                table: "Logins");

            migrationBuilder.RenameTable(
                name: "Logins",
                newName: "Login");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Login",
                table: "Login",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Login_LoginId",
                table: "Employees",
                column: "LoginId",
                principalTable: "Login",
                principalColumn: "Id");
        }
    }
}
