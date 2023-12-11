using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectumCommunity.Migrations
{
    /// <inheritdoc />
    public partial class EditPhoto3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"PersonalPhotos\" ALTER COLUMN \"Value\" TYPE bytea USING CAST(\"Value\" AS bytea)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "PersonalPhotos",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }
    }
}
