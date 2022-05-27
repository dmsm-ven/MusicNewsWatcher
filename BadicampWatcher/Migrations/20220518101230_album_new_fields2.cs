using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BandcampWatcher.Migrations
{
    public partial class album_new_fields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Albums",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Albums",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Albums");
        }
    }
}
