using MeetingsApi.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingsApi.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260811192000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Participants",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                Position = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Participants", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Location = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                Capacity = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Meetings",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                RoomId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Meetings", x => x.Id);
                table.ForeignKey(
                    name: "FK_Meetings_Rooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "MeetingParticipants",
            columns: table => new
            {
                MeetingId = table.Column<int>(type: "int", nullable: false),
                ParticipantId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MeetingParticipants", x => new { x.MeetingId, x.ParticipantId });
                table.ForeignKey(
                    name: "FK_MeetingParticipants_Meetings_MeetingId",
                    column: x => x.MeetingId,
                    principalTable: "Meetings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MeetingParticipants_Participants_ParticipantId",
                    column: x => x.ParticipantId,
                    principalTable: "Participants",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MeetingParticipants_ParticipantId",
            table: "MeetingParticipants",
            column: "ParticipantId");

        migrationBuilder.CreateIndex(
            name: "IX_Meetings_RoomId",
            table: "Meetings",
            column: "RoomId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "MeetingParticipants");
        migrationBuilder.DropTable(name: "Meetings");
        migrationBuilder.DropTable(name: "Participants");
        migrationBuilder.DropTable(name: "Rooms");
    }
}
