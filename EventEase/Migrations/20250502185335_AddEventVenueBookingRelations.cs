using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase.Migrations
{
    /// <inheritdoc />
    public partial class AddEventVenueBookingRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_VenueId_BookingDate",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VenueId",
                table: "Bookings",
                column: "VenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_VenueId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VenueId_BookingDate",
                table: "Bookings",
                columns: new[] { "VenueId", "BookingDate" },
                unique: true);
        }
    }
}
