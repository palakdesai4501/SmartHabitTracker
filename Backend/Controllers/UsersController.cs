using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HabitTracker.API.Data;
using HabitTracker.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
                return NotFound();
                
            var userToReturn = _mapper.Map<UserToReturnDto>(user);
            
            return Ok(userToReturn);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
                return NotFound();
                
            var userToReturn = _mapper.Map<UserToReturnDto>(user);
            
            return Ok(userToReturn);
        }
    }
}