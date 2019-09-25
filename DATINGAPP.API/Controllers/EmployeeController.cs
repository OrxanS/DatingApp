using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATINGAPP.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATINGAPP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            return Ok(await _context.Employees.ToListAsync());
        }
    }
}