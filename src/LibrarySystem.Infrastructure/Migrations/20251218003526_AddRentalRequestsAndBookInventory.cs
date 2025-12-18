using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalRequestsAndBookInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCount",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "InRentCount",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCount",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "RentalRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DenialReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProcessedByUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalRequests_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalRequests_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_BookId",
                table: "RentalRequests",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalRequests_ReaderId",
                table: "RentalRequests",
                column: "ReaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalRequests");

            migrationBuilder.DropColumn(
                name: "AvailableCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "InRentCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TotalCount",
                table: "Books");
        }
    }
}
