using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetings6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Meetings_PresidentId",
                table: "Meetings",
                column: "PresidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_SecretaryId",
                table: "Meetings",
                column: "SecretaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Employees_PresidentId",
                table: "Meetings",
                column: "PresidentId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Employees_SecretaryId",
                table: "Meetings",
                column: "SecretaryId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Employees_PresidentId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Employees_SecretaryId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_PresidentId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_SecretaryId",
                table: "Meetings");
        }
    }
}
