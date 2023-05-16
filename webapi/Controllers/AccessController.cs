using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : Controller
    {
        private readonly ClinicDbContext _db;
        private readonly UserManager<Patient> _userManager;
        private readonly IMapper _mapper;

        public AccessController(ClinicDbContext db, UserManager<Patient> userManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ValidateAntiForgeryToken]
        public async Task<GenericResponseDto<Patient>> SignUp(Patient obj)
        {
            var response = 

            await _db.Patients.AddAsync(obj);
            await _db.SaveChangesAsync();
            return CreatedResult(obj);
        }
    }
}
