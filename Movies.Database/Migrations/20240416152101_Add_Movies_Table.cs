using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movies.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Movies_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: false),
                    Length = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Img = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Genres",
                table: "Movies",
                column: "Genres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
