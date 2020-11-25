using Microsoft.EntityFrameworkCore.Migrations;

namespace LanchesMac.Migrations
{
    public partial class ItensPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalItensPedido",
                table: "Pedidos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalItensPedido",
                table: "Pedidos");
        }
    }
}
