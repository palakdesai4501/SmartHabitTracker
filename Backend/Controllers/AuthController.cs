using System.Threading.Tasks;
using AutoMapper;
using HabitTracker.API.DTOs;
using HabitTracker.API.Models;
using HabitTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            
            if (await _authService.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");
                
            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            
            var createdUser = await _authService.Register(userToCreate, userForRegisterDto.Password);
            
            var userToReturn = _mapper.Map<UserToReturnDto>(createdUser);
            
            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, userToReturn);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _authService.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            
            if (userFromRepo == null)
                return Unauthorized();
                
            var user = _mapper.Map<UserToReturnDto>(userFromRepo);
            
            return Ok(new
            {
                token = _authService.GenerateToken(userFromRepo),
                user
            });
        }
    }
}