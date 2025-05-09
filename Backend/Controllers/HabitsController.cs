using System.Collections.Generic;
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
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitService _habitService;
        private readonly IMapper _mapper;
        
        public HabitsController(IHabitService habitService, IMapper mapper)
        {
            _habitService = habitService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetHabits()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habitsFromRepo = await _habitService.GetHabitsForUser(userId);
            
            var habitsToReturn = new List<HabitDto>();
            
            foreach (var habit in habitsFromRepo)
            {
                var habitDto = _mapper.Map<HabitDto>(habit);
                habitDto.CurrentStreak = await _habitService.GetCurrentStreak(habit.Id);
                habitDto.LongestStreak = await _habitService.GetLongestStreak(habit.Id);
                habitDto.CompletionRate = await _habitService.GetCompletionRate(habit.Id);
                habitDto.CompletedDays = habit.HabitLogs?.Count(hl => hl.IsCompleted) ?? 0;
                
                habitsToReturn.Add(habitDto);
            }
            
            return Ok(habitsToReturn);
        }
        
        [HttpGet("{id}", Name = "GetHabit")]
        public async Task<IActionResult> GetHabit(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habitFromRepo = await _habitService.GetHabitById(id, userId);
            
            if (habitFromRepo == null)
                return NotFound();
                
            var habitToReturn = _mapper.Map<HabitDto>(habitFromRepo);
            habitToReturn.CurrentStreak = await _habitService.GetCurrentStreak(habitFromRepo.Id);
            habitToReturn.LongestStreak = await _habitService.GetLongestStreak(habitFromRepo.Id);
            habitToReturn.CompletionRate = await _habitService.GetCompletionRate(habitFromRepo.Id);
            habitToReturn.CompletedDays = habitFromRepo.HabitLogs?.Count(hl => hl.IsCompleted) ?? 0;
            
            return Ok(habitToReturn);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateHabit(HabitForCreationDto habitForCreationDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habitToCreate = _mapper.Map<Habit>(habitForCreationDto);
            habitToCreate.UserId = userId;
            
            var createdHabit = await _habitService.CreateHabit(habitToCreate);
            
            var habitToReturn = _mapper.Map<HabitDto>(createdHabit);
            
            return CreatedAtRoute("GetHabit", new { id = createdHabit.Id }, habitToReturn);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabit(int id, HabitForUpdateDto habitForUpdateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habitFromRepo = await _habitService.GetHabitById(id, userId);
            
            if (habitFromRepo == null)
                return NotFound();
                
            _mapper.Map(habitForUpdateDto, habitFromRepo);
            
            var updatedHabit = await _habitService.UpdateHabit(habitFromRepo);
            
            var habitToReturn = _mapper.Map<HabitDto>(updatedHabit);
            habitToReturn.CurrentStreak = await _habitService.GetCurrentStreak(updatedHabit.Id);
            habitToReturn.LongestStreak = await _habitService.GetLongestStreak(updatedHabit.Id);
            habitToReturn.CompletionRate = await _habitService.GetCompletionRate(updatedHabit.Id);
            habitToReturn.CompletedDays = updatedHabit.HabitLogs?.Count(hl => hl.IsCompleted) ?? 0;
            
            return Ok(habitToReturn);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var result = await _habitService.DeleteHabit(id, userId);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
    }
}