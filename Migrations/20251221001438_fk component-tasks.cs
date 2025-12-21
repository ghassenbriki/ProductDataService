using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leoni.Migrations
{
    /// <inheritdoc />
    public partial class fkcomponenttasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComponentName",
                table: "Tasks",
                type: "nvarchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ComponentName",
                table: "Tasks",
                column: "ComponentName");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Component_ComponentName",
                table: "Tasks",
                column: "ComponentName",
                principalTable: "Component",
                principalColumn: "ComponentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Component_ComponentName",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ComponentName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ComponentName",
                table: "Tasks");
        }
    }
}
