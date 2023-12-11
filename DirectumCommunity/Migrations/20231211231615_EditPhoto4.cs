using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditPhoto4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PersonalPhotos_PersonalPhotoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PersonalPhotoId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalPhotoId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhotoHash",
                table: "PersonalPhotos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalPhotoHash",
                table: "PersonalPhotos");

            migrationBuilder.AddColumn<int>(
                name: "PersonalPhotoId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonalPhotoId",
                table: "Employees",
                column: "PersonalPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_PersonalPhotos_PersonalPhotoId",
                table: "Employees",
                column: "PersonalPhotoId",
                principalTable: "PersonalPhotos",
                principalColumn: "Id");
        }
    }
}
