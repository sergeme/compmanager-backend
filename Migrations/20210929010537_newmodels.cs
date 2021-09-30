using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CompManager.Migrations
{
  public partial class newmodels : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "Title",
          table: "Accounts");

      migrationBuilder.AddColumn<int>(
          name: "CompetenceType",
          table: "Accounts",
          type: "integer",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.CreateTable(
          name: "Curricula",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Curricula", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Departments",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Departments", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Locations",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Locations", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Vocables",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Vocables", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Courses",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            DepartmentId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Courses", x => x.Id);
            table.ForeignKey(
                      name: "FK_Courses_Departments_DepartmentId",
                      column: x => x.DepartmentId,
                      principalTable: "Departments",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Tags",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            VocableId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Tags", x => x.Id);
            table.ForeignKey(
                      name: "FK_Tags_Vocables_VocableId",
                      column: x => x.VocableId,
                      principalTable: "Vocables",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Classes",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            DepartmentId = table.Column<int>(type: "integer", nullable: false),
            CourseId = table.Column<int>(type: "integer", nullable: false),
            CurriculumId = table.Column<int>(type: "integer", nullable: false),
            LocationId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Classes", x => x.Id);
            table.ForeignKey(
                      name: "FK_Classes_Courses_CourseId",
                      column: x => x.CourseId,
                      principalTable: "Courses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Classes_Curricula_CurriculumId",
                      column: x => x.CurriculumId,
                      principalTable: "Curricula",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Classes_Departments_DepartmentId",
                      column: x => x.DepartmentId,
                      principalTable: "Departments",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Classes_Locations_LocationId",
                      column: x => x.LocationId,
                      principalTable: "Locations",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "ProcessTypes",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            CourseId = table.Column<int>(type: "integer", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ProcessTypes", x => x.Id);
            table.ForeignKey(
                      name: "FK_ProcessTypes_Courses_CourseId",
                      column: x => x.CourseId,
                      principalTable: "Courses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "AccountClass",
          columns: table => new
          {
            AccountsId = table.Column<int>(type: "integer", nullable: false),
            ClassesId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AccountClass", x => new { x.AccountsId, x.ClassesId });
            table.ForeignKey(
                      name: "FK_AccountClass_Accounts_AccountsId",
                      column: x => x.AccountsId,
                      principalTable: "Accounts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_AccountClass_Classes_ClassesId",
                      column: x => x.ClassesId,
                      principalTable: "Classes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "CurriculumProcessType",
          columns: table => new
          {
            CurriculaId = table.Column<int>(type: "integer", nullable: false),
            ProcessTypesId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_CurriculumProcessType", x => new { x.CurriculaId, x.ProcessTypesId });
            table.ForeignKey(
                      name: "FK_CurriculumProcessType_Curricula_CurriculaId",
                      column: x => x.CurriculaId,
                      principalTable: "Curricula",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_CurriculumProcessType_ProcessTypes_ProcessTypesId",
                      column: x => x.ProcessTypesId,
                      principalTable: "ProcessTypes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Processes",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            Content = table.Column<string>(type: "text", nullable: true),
            ProcessTypeId = table.Column<int>(type: "integer", nullable: false),
            CurriculumId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Processes", x => x.Id);
            table.ForeignKey(
                      name: "FK_Processes_Curricula_CurriculumId",
                      column: x => x.CurriculumId,
                      principalTable: "Curricula",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Processes_ProcessTypes_ProcessTypeId",
                      column: x => x.ProcessTypeId,
                      principalTable: "ProcessTypes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Competences",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: true),
            ProcessId = table.Column<int>(type: "integer", nullable: false),
            AccountId = table.Column<int>(type: "integer", nullable: false),
            Action = table.Column<string>(type: "text", nullable: true),
            Context = table.Column<string>(type: "text", nullable: true),
            Description = table.Column<string>(type: "text", nullable: true),
            FinalResults = table.Column<string>(type: "text", nullable: true),
            SuccessCriteria = table.Column<string>(type: "text", nullable: true),
            BasicKnowledge = table.Column<string>(type: "text", nullable: true),
            CompetenceType = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Competences", x => x.Id);
            table.ForeignKey(
                      name: "FK_Competences_Accounts_AccountId",
                      column: x => x.AccountId,
                      principalTable: "Accounts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Competences_Processes_ProcessId",
                      column: x => x.ProcessId,
                      principalTable: "Processes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Comments",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Reviewer = table.Column<int>(type: "integer", nullable: false),
            Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            CompetenceId = table.Column<int>(type: "integer", nullable: false),
            Content = table.Column<string>(type: "text", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Comments", x => x.Id);
            table.ForeignKey(
                      name: "FK_Comments_Competences_CompetenceId",
                      column: x => x.CompetenceId,
                      principalTable: "Competences",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "CompetenceTag",
          columns: table => new
          {
            CompetencesId = table.Column<int>(type: "integer", nullable: false),
            TagsId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_CompetenceTag", x => new { x.CompetencesId, x.TagsId });
            table.ForeignKey(
                      name: "FK_CompetenceTag_Competences_CompetencesId",
                      column: x => x.CompetencesId,
                      principalTable: "Competences",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_CompetenceTag_Tags_TagsId",
                      column: x => x.TagsId,
                      principalTable: "Tags",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Reviews",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            CompetenceId = table.Column<int>(type: "integer", nullable: false),
            Reviewer = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Reviews", x => x.Id);
            table.ForeignKey(
                      name: "FK_Reviews_Competences_CompetenceId",
                      column: x => x.CompetenceId,
                      principalTable: "Competences",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_AccountClass_ClassesId",
          table: "AccountClass",
          column: "ClassesId");

      migrationBuilder.CreateIndex(
          name: "IX_Classes_CourseId",
          table: "Classes",
          column: "CourseId");

      migrationBuilder.CreateIndex(
          name: "IX_Classes_CurriculumId",
          table: "Classes",
          column: "CurriculumId");

      migrationBuilder.CreateIndex(
          name: "IX_Classes_DepartmentId",
          table: "Classes",
          column: "DepartmentId");

      migrationBuilder.CreateIndex(
          name: "IX_Classes_LocationId",
          table: "Classes",
          column: "LocationId");

      migrationBuilder.CreateIndex(
          name: "IX_Comments_CompetenceId",
          table: "Comments",
          column: "CompetenceId");

      migrationBuilder.CreateIndex(
          name: "IX_Competences_AccountId",
          table: "Competences",
          column: "AccountId");

      migrationBuilder.CreateIndex(
          name: "IX_Competences_ProcessId",
          table: "Competences",
          column: "ProcessId");

      migrationBuilder.CreateIndex(
          name: "IX_CompetenceTag_TagsId",
          table: "CompetenceTag",
          column: "TagsId");

      migrationBuilder.CreateIndex(
          name: "IX_Courses_DepartmentId",
          table: "Courses",
          column: "DepartmentId");

      migrationBuilder.CreateIndex(
          name: "IX_CurriculumProcessType_ProcessTypesId",
          table: "CurriculumProcessType",
          column: "ProcessTypesId");

      migrationBuilder.CreateIndex(
          name: "IX_Processes_CurriculumId",
          table: "Processes",
          column: "CurriculumId");

      migrationBuilder.CreateIndex(
          name: "IX_Processes_ProcessTypeId",
          table: "Processes",
          column: "ProcessTypeId");

      migrationBuilder.CreateIndex(
          name: "IX_ProcessTypes_CourseId",
          table: "ProcessTypes",
          column: "CourseId");

      migrationBuilder.CreateIndex(
          name: "IX_Reviews_CompetenceId",
          table: "Reviews",
          column: "CompetenceId");

      migrationBuilder.CreateIndex(
          name: "IX_Tags_VocableId",
          table: "Tags",
          column: "VocableId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "AccountClass");

      migrationBuilder.DropTable(
          name: "Comments");

      migrationBuilder.DropTable(
          name: "CompetenceTag");

      migrationBuilder.DropTable(
          name: "CurriculumProcessType");

      migrationBuilder.DropTable(
          name: "Reviews");

      migrationBuilder.DropTable(
          name: "Classes");

      migrationBuilder.DropTable(
          name: "Tags");

      migrationBuilder.DropTable(
          name: "Competences");

      migrationBuilder.DropTable(
          name: "Locations");

      migrationBuilder.DropTable(
          name: "Vocables");

      migrationBuilder.DropTable(
          name: "Processes");

      migrationBuilder.DropTable(
          name: "Curricula");

      migrationBuilder.DropTable(
          name: "ProcessTypes");

      migrationBuilder.DropTable(
          name: "Courses");

      migrationBuilder.DropTable(
          name: "Departments");

      migrationBuilder.DropColumn(
          name: "CompetenceType",
          table: "Accounts");

      migrationBuilder.AddColumn<string>(
          name: "Title",
          table: "Accounts",
          type: "text",
          nullable: true);
    }
  }
}
