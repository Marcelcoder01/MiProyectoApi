using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiProyectoApi.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedBy", "Date", "Name", "Photo", "Surname" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 9, 17, 17, 4, 23, 378, DateTimeKind.Local).AddTicks(2709), "Marcel", "blablabla", "Soto" },
                    { 2, 2, new DateTime(2023, 9, 17, 17, 4, 23, 378, DateTimeKind.Local).AddTicks(2755), "Alberto", "blablablaaaa", "Gonzalez" },
                    { 3, 3, new DateTime(2023, 9, 17, 17, 4, 23, 378, DateTimeKind.Local).AddTicks(2758), "Manuel", "blablablaaaaaa", "Esquinso" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
