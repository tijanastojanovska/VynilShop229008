using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VynilShop.Repository.Migrations
{
    public partial class artist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "Vynils",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Songs",
                table: "Vynils",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vynils_ArtistId",
                table: "Vynils",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vynils_Artists_ArtistId",
                table: "Vynils",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vynils_Artists_ArtistId",
                table: "Vynils");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Vynils_ArtistId",
                table: "Vynils");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Vynils");

            migrationBuilder.DropColumn(
                name: "Songs",
                table: "Vynils");
        }
    }
}
