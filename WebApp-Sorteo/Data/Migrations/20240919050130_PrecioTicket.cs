using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Sorteo.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrecioTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PrecioTicket",
                table: "Tickets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PrecioUnidad",
                table: "Sorteos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioTicket",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PrecioUnidad",
                table: "Sorteos");
        }
    }
}
