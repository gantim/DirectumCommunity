using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetings3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PresidentId",
                table: "Meetings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecretaryId",
                table: "Meetings",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresidentId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "SecretaryId",
                table: "Meetings");
        }
    }
}
