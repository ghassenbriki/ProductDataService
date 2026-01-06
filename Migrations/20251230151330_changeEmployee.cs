using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leoni.Migrations
{
    /// <inheritdoc />
    public partial class changeEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedByFk",
                table: "Tasks",
                type: "nvarchar(100)",
                nullable: true);


            migrationBuilder.AddColumn<string>(
                name: "HashedPasword",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedByFk",
                table: "Tasks",
                column: "AssignedByFk");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AssignedBy",
                table: "Tasks",
                column: "AssignedByFk",
                principalTable: "Employee",
                principalColumn: "sessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AssignedBy",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedByFk",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssignedByFk",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "HashedPasword",
                table: "Employee");
        }
    }
}
