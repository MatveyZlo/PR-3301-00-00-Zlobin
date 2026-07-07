using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    /// <summary>
    /// Контроллер для управления задачами.
    /// </summary>
   
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список задач.
        /// </summary>
        
        // Получить все задачи
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTask>>> GetTasks()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .ToListAsync();
        }

        /// <summary>
        /// Создать новую задачу.
        /// </summary>
        
        // Получить задачу по Id
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkTask>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return task;
        }

        /// <summary>
        /// Добавить задачу.
        /// </summary>

        // Добавить задачу
        [HttpPost]
        public async Task<ActionResult<WorkTask>> CreateTask(WorkTask task)
        {
            var project = await _context.Projects.FindAsync(task.ProjectId);

            if (project == null)
                return BadRequest("Проект не найден.");

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        /// <summary>
        /// Изменить задачу.
        /// </summary>

        // Изменить задачу
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, WorkTask task)
        {
            if (id != task.Id)
                return BadRequest();

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить задачу.
        /// </summary>
        
        // Удалить задачу
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}