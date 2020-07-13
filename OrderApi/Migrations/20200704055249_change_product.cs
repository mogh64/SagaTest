using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderApi.Migrations
{
    public partial class change_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "OrderApi",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "OrderApi",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                schema: "OrderApi",
                table: "Product",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                schema: "OrderApi",
                table: "Product");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "OrderApi",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "OrderApi",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
