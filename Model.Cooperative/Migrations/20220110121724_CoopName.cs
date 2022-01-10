using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Cooperative.Migrations
{
    public partial class CoopName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coop",
                columns: table => new
                {
                    IdCoop = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoopName = table.Column<string>(nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coop", x => x.IdCoop);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    IdNumber = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "StepProject",
                columns: table => new
                {
                    StepProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NbreOfDays = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StepBuget = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ReviewDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepProject", x => x.StepProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Efficiency = table.Column<int>(nullable: false),
                    DurationInMonth = table.Column<int>(nullable: false),
                    ProjectBudget = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CoopIdCoop = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Project_Coop_CoopIdCoop",
                        column: x => x.CoopIdCoop,
                        principalTable: "Coop",
                        principalColumn: "IdCoop",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Membre",
                columns: table => new
                {
                    MembreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeesPerYear = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PersonId = table.Column<int>(nullable: true),
                    CoopIdCoop = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membre", x => x.MembreId);
                    table.ForeignKey(
                        name: "FK_Membre_Coop_CoopIdCoop",
                        column: x => x.CoopIdCoop,
                        principalTable: "Coop",
                        principalColumn: "IdCoop",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Membre_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ManagerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectBudget = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ExpenseBudget = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AfterStepBudget = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Manager_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Manager_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ManagerId = table.Column<int>(nullable: true),
                    PersonId = table.Column<int>(nullable: true),
                    StepProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_StepProject_StepProjectId",
                        column: x => x.StepProjectId,
                        principalTable: "StepProject",
                        principalColumn: "StepProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerId",
                table: "Employee",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PersonId",
                table: "Employee",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_StepProjectId",
                table: "Employee",
                column: "StepProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_PersonId",
                table: "Manager",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_ProjectId",
                table: "Manager",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_CoopIdCoop",
                table: "Membre",
                column: "CoopIdCoop");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_PersonId",
                table: "Membre",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CoopIdCoop",
                table: "Project",
                column: "CoopIdCoop");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Membre");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "StepProject");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Coop");
        }
    }
}
