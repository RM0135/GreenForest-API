using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenForest.Migrations
{
    /// <inheritdoc />
    public partial class AgregarHumedadEspecies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HumedadMaxima",
                table: "Especies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HumedadMinima",
                table: "Especies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HumedadMaxima",
                table: "Especies");

            migrationBuilder.DropColumn(
                name: "HumedadMinima",
                table: "Especies");
        }
    }
}
