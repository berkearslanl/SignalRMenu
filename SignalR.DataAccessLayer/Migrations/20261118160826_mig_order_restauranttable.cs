using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalR.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_order_restauranttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableNumber",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantTableId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RestaurantTables",
                columns: table => new
                {
                    RestaurantTableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantTables", x => x.RestaurantTableId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantTableId",
                table: "Orders",
                column: "RestaurantTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RestaurantTables_RestaurantTableId",
                table: "Orders",
                column: "RestaurantTableId",
                principalTable: "RestaurantTables",
                principalColumn: "RestaurantTableId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RestaurantTables_RestaurantTableId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "RestaurantTables");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RestaurantTableId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantTableId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "TableNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
