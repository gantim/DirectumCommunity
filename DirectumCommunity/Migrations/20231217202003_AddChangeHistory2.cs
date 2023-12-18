using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "PersonChanges",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PersonChanges_PersonId",
                table: "PersonChanges",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonChanges_Persons_PersonId",
                table: "PersonChanges",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonChanges_Persons_PersonId",
                table: "PersonChanges");

            migrationBuilder.DropIndex(
                name: "IX_PersonChanges_PersonId",
                table: "PersonChanges");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "PersonChanges");
        }
    }
}
