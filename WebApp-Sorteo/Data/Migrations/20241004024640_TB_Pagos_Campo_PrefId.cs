using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Sorteo.Data.Migrations
{
    /// <inheritdoc />
    public partial class TB_Pagos_Campo_PrefId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrefId",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrefId",
                table: "Pagos");
        }
    }
}
