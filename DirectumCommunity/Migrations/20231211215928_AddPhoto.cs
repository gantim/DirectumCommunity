using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonalPhotoId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonalPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalPhotos", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PersonalPhotos_PersonalPhotoId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "PersonalPhotos");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PersonalPhotoId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PersonalPhotoId",
                table: "Employees");
        }
    }
}
