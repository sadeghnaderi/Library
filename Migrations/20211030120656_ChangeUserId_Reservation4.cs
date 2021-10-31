using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class ChangeUserId_Reservation4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_AspNetUsers_UserId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservation");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReservationId",
                table: "AspNetUsers",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Reservation_ReservationId",
                table: "AspNetUsers",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Reservation_ReservationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReservationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reservation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_AspNetUsers_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
