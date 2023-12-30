using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "MeetingMembers",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "integer", nullable: false),
                    MeetingsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingMembers", x => new { x.EmployeesId, x.MeetingsId });
                    table.ForeignKey(
                        name: "FK_MeetingMembers_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingMembers_Meetings_MeetingsId",
                        column: x => x.MeetingsId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingMembers_MeetingsId",
                table: "MeetingMembers",
                column: "MeetingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingMembers");

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
    }
}
