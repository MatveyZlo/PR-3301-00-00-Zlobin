using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    /// <summary>
    /// Контроллер учета рабочего времени.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimeEntriesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить статус рабочего дня.
        /// </summary>
        [HttpGet("status/{date}")]
        public async Task<ActionResult<object>> GetStatus(DateOnly date)
        {
            decimal hours = await _context.TimeEntries
                .Where(e => e.Date == date)
                .SumAsync(e => e.Hours);

            string sticker = hours switch
            {
                < 8 => "Yellow",
                8 => "Green",
                _ => "Red"
            };

            return Ok(new
            {
                Date = date,
                TotalHours = hours,
                Sticker = sticker
            });
        }

        /// <summary>
        /// Получить все проводки.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetEntries()
        {
            return await _context.TimeEntries
                .Include(e => e.WorkTask)
                .ThenInclude(t => t.Project)
                .ToListAsync();
        }

        /// <summary>
        /// Получить проводки за выбранный день.
        /// </summary>
        [HttpGet("day/{date}")]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetByDay(DateOnly date)
        {
            return await _context.TimeEntries
                .Include(e => e.WorkTask)
                .Where(e => e.Date == date)
                .ToListAsync();
        }

        /// <summary>
        /// Получить проводки за выбранный месяц.
        /// </summary>
        [HttpGet("month/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetByMonth(int year, int month)
        {
            return await _context.TimeEntries
                .Include(e => e.WorkTask)
                .Where(e => e.Date.Year == year && e.Date.Month == month)
                .ToListAsync();
        }

        /// <summary>
        /// Добавить новую проводку.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TimeEntry>> CreateEntry(TimeEntry entry)
        {
            var task = await _context.Tasks.FindAsync(entry.WorkTaskId);

            if (task == null)
                return BadRequest("Задача не найдена.");

            if (!task.IsActive)
                return BadRequest("Нельзя выбрать неактивную задачу.");

            decimal totalHours = await _context.TimeEntries
                .Where(e => e.Date == entry.Date)
                .SumAsync(e => e.Hours);

            if (totalHours + entry.Hours > 24)
                return BadRequest("За один день нельзя списать более 24 часов.");

            _context.TimeEntries.Add(entry);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntries), new { id = entry.Id }, entry);
        }

        /// <summary>
        /// Изменить существующую проводку.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntry(int id, TimeEntry entry)
        {
            if (id != entry.Id)
                return BadRequest();

            var oldEntry = await _context.TimeEntries
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (oldEntry == null)
                return NotFound();

            // Если пользователь пытается изменить задачу
            if (oldEntry.WorkTaskId != entry.WorkTaskId)
            {
                var oldTask = await _context.Tasks.FindAsync(oldEntry.WorkTaskId);

                // Если старая задача уже неактивна — менять её нельзя
                if (oldTask != null && !oldTask.IsActive)
                {
                    return BadRequest("Нельзя изменить задачу у проводки, так как она уже неактивна.");
                }
            }

            // Проверяем новую задачу
            var newTask = await _context.Tasks.FindAsync(entry.WorkTaskId);

            if (newTask == null)
                return BadRequest("Задача не найдена.");

            if (!newTask.IsActive)
                return BadRequest("Нельзя выбрать неактивную задачу.");

            // Проверяем сумму часов за день
            decimal totalHours = await _context.TimeEntries
                .Where(e => e.Date == entry.Date && e.Id != id)
                .SumAsync(e => e.Hours);

            if (totalHours + entry.Hours > 24)
                return BadRequest("За один день нельзя списать более 24 часов.");

            _context.Entry(entry).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить проводку.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await _context.TimeEntries.FindAsync(id);

            if (entry == null)
                return NotFound();

            _context.TimeEntries.Remove(entry);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}