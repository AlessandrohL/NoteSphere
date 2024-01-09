using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NotesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "DeleteAt", "IsDeleted", "ModifiedAt", "NoteBookId", "Title" },
                values: new object[,]
                {
                    { new Guid("10aab93a-ab94-41fc-a205-a3f9afd354a8"), "Contenido de ejemplo", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Nota SK" },
                    { new Guid("428646ff-29c6-4f7f-a9e9-c57a9afa73c6"), "Contenido de la novena nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Novena Nota" },
                    { new Guid("54797698-e18a-4ae2-a1da-4d4daf4ca919"), "Contenido de la cuarta nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Cuarta Nota" },
                    { new Guid("59a761db-11ad-4979-9408-4bba1c891067"), "Contenido de la séptima nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Séptima Nota" },
                    { new Guid("7b4affff-ba13-42ab-86f0-45d315d4f85f"), "Contenido de la octava nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Octava Nota" },
                    { new Guid("7c168744-34d5-4e91-9dbd-e8d52dcb9dd4"), "Contenido de la decimocuarta nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Decimocuarta Nota" },
                    { new Guid("9868fca0-774c-453c-ba35-59b24dc53bbb"), "Contenido de la décima nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Décima Nota" },
                    { new Guid("a68e5a62-46f3-47e8-85dc-afbb82f0ae31"), "Contenido de la segunda nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Segunda Nota" },
                    { new Guid("c69cb328-82d6-4691-bbcb-5c4a7cd7a58e"), "Contenido de la decimotercera nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Decimotercera Nota" },
                    { new Guid("d7f83b45-4109-4658-b433-7fd23646c426"), "Contenido de la sexta nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Sexta Nota" },
                    { new Guid("df90d467-0ade-4415-bfda-03bb3171a2fd"), "Contenido de la duodécima nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Duodécima Nota" },
                    { new Guid("e14cbc20-6ac6-4dda-98fd-37ac877b36a0"), "Contenido de la tercera nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Tercera Nota" },
                    { new Guid("eb29ed64-e904-46a5-9766-340fdd63b250"), "Contenido de la decimoquinta nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Decimoquinta Nota" },
                    { new Guid("f9a6be30-4563-4647-93c3-de0c774aff28"), "Contenido de la undécima nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Undécima Nota" },
                    { new Guid("fc2af002-02e3-4472-84a5-646dfcc1d761"), "Contenido de la quinta nota", null, false, null, new Guid("cdc4756e-b9bb-4c08-d303-08dc0e447268"), "Quinta Nota" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("10aab93a-ab94-41fc-a205-a3f9afd354a8"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("428646ff-29c6-4f7f-a9e9-c57a9afa73c6"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("54797698-e18a-4ae2-a1da-4d4daf4ca919"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("59a761db-11ad-4979-9408-4bba1c891067"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("7b4affff-ba13-42ab-86f0-45d315d4f85f"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("7c168744-34d5-4e91-9dbd-e8d52dcb9dd4"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("9868fca0-774c-453c-ba35-59b24dc53bbb"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("a68e5a62-46f3-47e8-85dc-afbb82f0ae31"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("c69cb328-82d6-4691-bbcb-5c4a7cd7a58e"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("d7f83b45-4109-4658-b433-7fd23646c426"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("df90d467-0ade-4415-bfda-03bb3171a2fd"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("e14cbc20-6ac6-4dda-98fd-37ac877b36a0"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("eb29ed64-e904-46a5-9766-340fdd63b250"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("f9a6be30-4563-4647-93c3-de0c774aff28"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("fc2af002-02e3-4472-84a5-646dfcc1d761"));
        }
    }
}
