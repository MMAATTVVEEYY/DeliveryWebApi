using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryWebApi.Migrations
{
    /// <inheritdoc />
    public partial class DeliveryIdisNowKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Deliveries",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Deliveries",
                newName: "id");
        }
    }
}
