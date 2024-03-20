using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sophia.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Personality = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                });

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
                    Location = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persona_Facts",
                columns: table => new
                {
                    PersonaEntityId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValueTemplate = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DefaultText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona_Facts", x => new { x.PersonaEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_Persona_Facts_Personas_PersonaEntityId",
                        column: x => x.PersonaEntityId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persona_Tools",
                columns: table => new
                {
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    ToolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona_Tools", x => new { x.PersonaId, x.ToolId });
                    table.ForeignKey(
                        name: "FK_Persona_Tools_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persona_Tools_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonaEntityToolEntity",
                columns: table => new
                {
                    KnownToolsId = table.Column<int>(type: "int", nullable: false),
                    PersonaEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaEntityToolEntity", x => new { x.KnownToolsId, x.PersonaEntityId });
                    table.ForeignKey(
                        name: "FK_PersonaEntityToolEntity_Personas_PersonaEntityId",
                        column: x => x.PersonaEntityId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonaEntityToolEntity_Tools_KnownToolsId",
                        column: x => x.KnownToolsId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tool_Arguments",
                columns: table => new
                {
                    ToolEntityId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<long>(type: "bigint", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    Choices = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool_Arguments", x => new { x.ToolEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_Tool_Arguments_Tools_ToolEntityId",
                        column: x => x.ToolEntityId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "World_Facts",
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
                    table.PrimaryKey("PK_World_Facts", x => new { x.WorldEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_World_Facts_Worlds_WorldEntityId",
                        column: x => x.WorldEntityId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "World_Tools",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_World_Tools", x => new { x.ToolId, x.WorldId });
                    table.ForeignKey(
                        name: "FK_World_Tools_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_World_Tools_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "World_User",
                columns: table => new
                {
                    WorldEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_World_User", x => x.WorldEntityId);
                    table.ForeignKey(
                        name: "FK_World_User_Worlds_WorldEntityId",
                        column: x => x.WorldEntityId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Worlds",
                columns: new[] { "Id", "Location" },
                values: new object[] { new Guid("49381b5a-a76b-486f-ac5c-b2807cff9675"), null });

            migrationBuilder.CreateIndex(
                name: "IX_Persona_Tools_ToolId",
                table: "Persona_Tools",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaEntityToolEntity_PersonaEntityId",
                table: "PersonaEntityToolEntity",
                column: "PersonaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_World_Tools_WorldId",
                table: "World_Tools",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persona_Facts");

            migrationBuilder.DropTable(
                name: "Persona_Tools");

            migrationBuilder.DropTable(
                name: "PersonaEntityToolEntity");

            migrationBuilder.DropTable(
                name: "Tool_Arguments");

            migrationBuilder.DropTable(
                name: "World_Facts");

            migrationBuilder.DropTable(
                name: "World_Tools");

            migrationBuilder.DropTable(
                name: "World_User");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "Worlds");
        }
    }
}
