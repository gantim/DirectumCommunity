using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetings4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMembers_Employees_EmployeesId",
                table: "MeetingMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingMembers",
                table: "MeetingMembers");

            migrationBuilder.DropIndex(
                name: "IX_MeetingMembers_MeetingsId",
                table: "MeetingMembers");

            migrationBuilder.RenameColumn(
                name: "EmployeesId",
                table: "MeetingMembers",
                newName: "MembersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingMembers",
                table: "MeetingMembers",
                columns: new[] { "MeetingsId", "MembersId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingMembers_MembersId",
                table: "MeetingMembers",
                column: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMembers_Employees_MembersId",
                table: "MeetingMembers",
                column: "MembersId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMembers_Employees_MembersId",
                table: "MeetingMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingMembers",
                table: "MeetingMembers");

            migrationBuilder.DropIndex(
                name: "IX_MeetingMembers_MembersId",
                table: "MeetingMembers");

            migrationBuilder.RenameColumn(
                name: "MembersId",
                table: "MeetingMembers",
                newName: "EmployeesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingMembers",
                table: "MeetingMembers",
                columns: new[] { "EmployeesId", "MeetingsId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingMembers_MeetingsId",
                table: "MeetingMembers",
                column: "MeetingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMembers_Employees_EmployeesId",
                table: "MeetingMembers",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
