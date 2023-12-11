using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PersonalPhotos",
                newName: "Photo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "PersonalPhotos",
                newName: "Value");
        }
    }
}
