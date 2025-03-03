using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using EMS.Models.ViewModels;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EMS.Models.Context; // EMSContext'in bulunduğu namespace

namespace EMS.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly DepartmentStatisticsContext _context;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReportsController(DepartmentStatisticsContext context, IConfiguration configuration)
        {
            _context = context;
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        [HttpGet("department-statistics")]
        public async Task<IActionResult> GetDepartmentStatistics()
        {
            try 
            {
                var statistics = await _context.Database.SqlQueryRaw<DepartmentStatisticsViewModel>("EXEC sp_GetDepartmentStatistics").ToListAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        
    }
}