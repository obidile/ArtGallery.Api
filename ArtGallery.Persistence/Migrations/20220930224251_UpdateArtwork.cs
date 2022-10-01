using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGallery.Persistence.Migrations
{
    public partial class UpdateArtwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtImage",
                table: "ArtWorks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtImage",
                table: "ArtWorks");
        }
    }
}
