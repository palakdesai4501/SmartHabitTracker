using AutoMapper;
using HabitTracker.API.DTOs;
using HabitTracker.API.Models;

namespace HabitTracker.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // User mappings
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserToReturnDto>();
            
            // Habit mappings
            CreateMap<HabitForCreationDto, Habit>();
            CreateMap<HabitForUpdateDto, Habit>();
            CreateMap<Habit, HabitDto>();
            
            // HabitLog mappings
            CreateMap<HabitLogForCreationDto, HabitLog>();
            CreateMap<HabitLogForUpdateDto, HabitLog>();
            CreateMap<HabitLog, HabitLogDto>();
        }
    }
}