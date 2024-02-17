using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PeopleIncApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateDataBasev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "Pessoa",
                keyColumn: "Id",
                keyValue: 99L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pessoa",
                columns: new[] { "Id", "Email", "Idade", "Nome" },
                values: new object[,]
                {
                    { 4L, "Kory47@yahoo.com", 53, "Keara" },
                    { 12L, "Lonnie_Schultz@yahoo.com", 39, "Dedric" },
                    { 29L, "Adele.VonRueden80@yahoo.com", 49, "Cecelia" },
                    { 51L, "Kasey_Pfannerstill62@yahoo.com", 37, "Alessandra" },
                    { 54L, "Kennedy68@yahoo.com", 55, "Maxine" },
                    { 79L, "Barry.Friesen34@yahoo.com", 41, "Rodolfo" },
                    { 87L, "Amaya.Muller@gmail.com", 33, "Arch" },
                    { 93L, "Rasheed_Koss48@gmail.com", 38, "Jordi" },
                    { 95L, "Delbert_Sauer53@hotmail.com", 29, "Dandre" },
                    { 99L, "Earline2@gmail.com", 46, "Ford" }
                });
        }
    }
}
