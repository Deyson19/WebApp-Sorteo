using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Sorteo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_TB_Ticket_EstadoTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoTicket",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoTicket",
                table: "Tickets");
        }
    }
}
