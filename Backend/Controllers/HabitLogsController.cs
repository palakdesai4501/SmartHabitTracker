using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HabitTracker.API.DTOs;
using HabitTracker.API.Models;
using HabitTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.API.Controllers
{
    [Authorize]
    [Route("api/habits/{habitId}/logs")]
    [ApiController]
    public class HabitLogsController : ControllerBase
    {
        private readonly IHabitService _habitService;
        private readonly IHabitLogService _habitLogService;
        private readonly IMapper _mapper;
        
        public HabitLogsController(IHabitService habitService, IHabitLogService habitLogService, IMapper mapper)
        {
            _habitService = habitService;
            _habitLogService = habitLogService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetHabitLogs(int habitId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = await _habitService.GetHabitById(habitId, userId);
            
            if (habit == null)
                return NotFound();
                
            var habitLogs = await _habitLogService.GetHabitLogsForHabit(habitId);
            
            var habitLogsToReturn = _mapper.Map<HabitLogDto[]>(habitLogs);
            
            return Ok(habitLogsToReturn);
        }
        
        [HttpGet("{id}", Name = "GetHabitLog")]
        public async Task<IActionResult> GetHabitLog(int habitId, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = await _habitService.GetHabitById(habitId, userId);
            
            if (habit == null)
                return NotFound();
                
            var habitLog = await _habitLogService.GetHabitLogById(id);
            
            if (habitLog == null || habitLog.HabitId != habitId)
                return NotFound();
                
            var habitLogToReturn = _mapper.Map<HabitLogDto>(habitLog);
            
            return Ok(habitLogToReturn);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateHabitLog(int habitId, HabitLogForCreationDto habitLogForCreationDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = await _habitService.GetHabitById(habitId, userId);
            
            if (habit == null)
                return NotFound();
                
            var habitLog = _mapper.Map<HabitLog>(habitLogForCreationDto);
            habitLog.HabitId = habitId;
            
            var createdHabitLog = await _habitLogService.CreateHabitLog(habitLog);
            
            var habitLogToReturn = _mapper.Map<HabitLogDto>(createdHabitLog);
            
            return CreatedAtRoute("GetHabitLog", new { habitId = habitId, id = createdHabitLog.Id }, habitLogToReturn);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabitLog(int habitId, int id, HabitLogForUpdateDto habitLogForUpdateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = await _habitService.GetHabitById(habitId, userId);
            
            if (habit == null)
                return NotFound();
                
            var habitLog = await _habitLogService.GetHabitLogById(id);
            
            if (habitLog == null || habitLog.HabitId != habitId)
                return NotFound();
                
            _mapper.Map(habitLogForUpdateDto, habitLog);
            
            var updatedHabitLog = await _habitLogService.UpdateHabitLog(habitLog);
            
            var habitLogToReturn = _mapper.Map<HabitLogDto>(updatedHabitLog);
            
            return Ok(habitLogToReturn);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitLog(int habitId, int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = await _habitService.GetHabitById(habitId, userId);
            
            if (habit == null)
                return NotFound();
                
            var habitLog = await _habitLogService.GetHabitLogById(id);
            
            if (habitLog == null || habitLog.HabitId != habitId)
                return NotFound();
                
            var result = await _habitLogService.DeleteHabitLog(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        [HttpGet("range")]
        public async Task<IActionResult> GetHabitLogsForRange(int habitId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = await _habitService.GetHabitById(habitId, userId);
            
            if (habit == null)
                return NotFound();
                
            var habitLogs = await _habitLogService.GetHabitLogsForDateRange(habitId, startDate, endDate);
            
            var habitLogsToReturn = _mapper.Map<HabitLogDto[]>(habitLogs);
            
            return Ok(habitLogsToReturn);
        }
    }
}