using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Meetings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Employees_EmployeeId",
                table: "Meetings",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Employees_EmployeeId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_EmployeeId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Meetings");
        }
    }
}
