using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HabitTracker.API.Data;
using HabitTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.API.Services
{
    public interface IHabitLogService
    {
        Task<IEnumerable<HabitLog>> GetHabitLogsForHabit(int habitId);
        Task<HabitLog> GetHabitLogById(int id);
        Task<HabitLog> GetHabitLogByDate(int habitId, DateTime date);
        Task<HabitLog> CreateHabitLog(HabitLog habitLog);
        Task<HabitLog> UpdateHabitLog(HabitLog habitLog);
        Task<bool> DeleteHabitLog(int id);
        Task<IEnumerable<HabitLog>> GetHabitLogsForDateRange(int habitId, DateTime startDate, DateTime endDate);
    }
    
    public class HabitLogService : IHabitLogService
    {
        private readonly ApplicationDbContext _context;
        
        public HabitLogService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<HabitLog>> GetHabitLogsForHabit(int habitId)
        {
            return await _context.HabitLogs
                .Where(hl => hl.HabitId == habitId)
                .OrderByDescending(hl => hl.Date)
                .ToListAsync();
        }
        
        public async Task<HabitLog> GetHabitLogById(int id)
        {
            return await _context.HabitLogs.FindAsync(id);
        }
        
        public async Task<HabitLog> GetHabitLogByDate(int habitId, DateTime date)
        {
            return await _context.HabitLogs
                .FirstOrDefaultAsync(hl => hl.HabitId == habitId && hl.Date.Date == date.Date);
        }
        
        public async Task<HabitLog> CreateHabitLog(HabitLog habitLog)
        {
            var existingLog = await GetHabitLogByDate(habitLog.HabitId, habitLog.Date);
            
            if (existingLog != null)
            {
                existingLog.IsCompleted = habitLog.IsCompleted;
                existingLog.Notes = habitLog.Notes;
                
                _context.HabitLogs.Update(existingLog);
                await _context.SaveChangesAsync();
                
                return existingLog;
            }
            
            await _context.HabitLogs.AddAsync(habitLog);
            await _context.SaveChangesAsync();
            
            return habitLog;
        }
        
        public async Task<HabitLog> UpdateHabitLog(HabitLog habitLog)
        {
            _context.HabitLogs.Update(habitLog);
            await _context.SaveChangesAsync();
            
            return habitLog;
        }
        
        public async Task<bool> DeleteHabitLog(int id)
        {
            var habitLog = await _context.HabitLogs.FindAsync(id);
            
            if (habitLog == null)
                return false;
                
            _context.HabitLogs.Remove(habitLog);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<IEnumerable<HabitLog>> GetHabitLogsForDateRange(int habitId, DateTime startDate, DateTime endDate)
        {
            return await _context.HabitLogs
                .Where(hl => hl.HabitId == habitId && hl.Date.Date >= startDate.Date && hl.Date.Date <= endDate.Date)
                .OrderBy(hl => hl.Date)
                .ToListAsync();
        }
    }
}