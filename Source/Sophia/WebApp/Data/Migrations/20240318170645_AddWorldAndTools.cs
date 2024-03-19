#nullable disable

namespace Sophia.WebApp.Migrations;

/// <inheritdoc />
public partial class AddWorldAndTools : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Tools",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tools", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Worlds",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Location = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                UserProfile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Worlds", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ArgumentEntity",
            columns: table => new
            {
                ToolEntityId = table.Column<int>(type: "int", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Index = table.Column<long>(type: "bigint", nullable: false),
                IsRequired = table.Column<bool>(type: "bit", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Type = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                Choices = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ArgumentEntity", x => new { x.ToolEntityId, x.Id });
                table.ForeignKey(
                    name: "FK_ArgumentEntity_Tools_ToolEntityId",
                    column: x => x.ToolEntityId,
                    principalTable: "Tools",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AvailableTools",
            columns: table => new
            {
                ToolId = table.Column<int>(type: "int", nullable: false),
                WorldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AvailableTools", x => new { x.ToolId, x.WorldId });
                table.ForeignKey(
                    name: "FK_AvailableTools_Tools_ToolId",
                    column: x => x.ToolId,
                    principalTable: "Tools",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AvailableTools_Worlds_WorldId",
                    column: x => x.WorldId,
                    principalTable: "Worlds",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FactEntity",
            columns: table => new
            {
                WorldEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ValueTemplate = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                DefaultText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InformationEntity", x => new { x.WorldEntityId, x.Id });
                table.ForeignKey(
                    name: "FK_InformationEntity_Worlds_WorldEntityId",
                    column: x => x.WorldEntityId,
                    principalTable: "Worlds",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AvailableTools_WorldId",
            table: "AvailableTools",
            column: "WorldId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ArgumentEntity");

        migrationBuilder.DropTable(
            name: "AvailableTools");

        migrationBuilder.DropTable(
            name: "FactEntity");

        migrationBuilder.DropTable(
            name: "Tools");

        migrationBuilder.DropTable(
            name: "Worlds");
    }
}
