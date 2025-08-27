using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly TaskContext _context;

        public TaskItemsController(TaskContext context)
        {
            _context = context;
        }

        // GET: TaskItems
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string statusFilter, int? pageNumber)
        {
			ViewData["CurrentSort"] = sortOrder;
			ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
			ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

			if (searchString != null || statusFilter != null)
			{
				pageNumber = 1; // Reset to page 1 when filter changes
			}
			else
			{
				searchString = currentFilter;
			}

			ViewData["CurrentFilter"] = searchString;
			ViewData["CurrentStatus"] = statusFilter;

			var tasks = from t in _context.Tasks
						select t;

			// Filtering by search
			if (!String.IsNullOrEmpty(searchString))
			{
				tasks = tasks.Where(t => t.Title.Contains(searchString)
									  || t.Description.Contains(searchString));
			}

			// Filtering by status
			if (!String.IsNullOrEmpty(statusFilter))
			{
				if (statusFilter == "Complete")
					tasks = tasks.Where(t => t.IsCompleted == true);
				else if (statusFilter == "Incomplete")
					tasks = tasks.Where(t => t.IsCompleted == false);
			}

			// Sorting
			switch (sortOrder)
			{
				case "title_desc":
					tasks = tasks.OrderByDescending(t => t.Title);
					break;
				case "Date":
					tasks = tasks.OrderBy(t => t.DueDate);
					break;
				case "date_desc":
					tasks = tasks.OrderByDescending(t => t.DueDate);
					break;
				default:
					tasks = tasks.OrderBy(t => t.Title);
					break;
			}

			int pageSize = 5;

			return View(await PaginatedList<TaskItem>.CreateAsync(tasks.AsNoTracking(), pageNumber ?? 1, pageSize));
		}

        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,IsCompleted")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,IsCompleted")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem != null)
            {
                _context.Tasks.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleComplete(int id)
		{
			var task = await _context.Tasks.FindAsync(id);
			if (task == null)
			{
				return NotFound();
			}

			task.IsCompleted = !task.IsCompleted; // Flip status
			_context.Update(task);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		private bool TaskItemExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
