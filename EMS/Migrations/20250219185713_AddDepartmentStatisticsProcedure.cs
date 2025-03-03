using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentStatisticsProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DROP PROCEDURE IF EXISTS sp_GetDepartmentStatistics;
        GO

        CREATE PROCEDURE sp_GetDepartmentStatistics
        AS
        BEGIN
            SELECT 
                d.Id AS DepartmentId,
                d.Name AS DepartmentName,
                COUNT(e.Id) AS EmployeeCount,
                ISNULL(AVG(CAST(e.Salary AS DECIMAL(18,2))),0) AS AverageSalary
            FROM Departments d
            LEFT JOIN Employees e ON d.Id = e.DepartmentId
            GROUP BY d.Id, d.Name
            ORDER BY d.Name;
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetDepartmentStatistics");
        }
    }
}
