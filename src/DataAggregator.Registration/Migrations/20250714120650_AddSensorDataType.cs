using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAggregator.Registration.Migrations
{
    /// <inheritdoc />
    public partial class AddSensorDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "Sensors",
                type: "integer",
                nullable: false,
                defaultValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Sensors");
        }
    }
}
