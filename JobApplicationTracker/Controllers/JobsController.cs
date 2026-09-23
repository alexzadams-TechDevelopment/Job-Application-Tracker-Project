
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.Models;
using JobApplicationTracker.Data;
using Microsoft.AspNetCore.Identity;

public class JobsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: JOBS
    public async Task<IActionResult> Index()    
    {
        //return View(await _context.Job.ToListAsync());

        var userId = _userManager.GetUserId(User);

        var jobs = await _context.Job
            .Where(j => j.UserId == userId) //To only retreive the data based on the account.
            .ToListAsync();

        return View(jobs);
    }

    // GET: JOBS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var job = await _context.Job
            .FirstOrDefaultAsync(m => m.Id == id);
        if (job == null)
        {
            return NotFound();
        }

        return View(job);
    }

    // GET: JOBS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: JOBS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,JobName,JobDescription,JobStatus, Location, JobUrl, DateApplied, AdditionalNotes")] Job job)
    {



        if (ModelState.IsValid)
        {
            job.UserId = _userManager.GetUserId(User);

            _context.Add(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(job);
    }

    // GET: JOBS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var job = await _context.Job.FindAsync(id);
        if (job == null)
        {
            return NotFound();
        }
        return View(job);
    }

    // POST: JOBS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,JobName,JobDescription,JobStatus, Location, JobUrl, DateApplied, AdditionalNotes")] Job job)
    {
        if (id != job.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(job);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(job.Id))
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
        return View(job);
    }

    // GET: JOBS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var job = await _context.Job
            .FirstOrDefaultAsync(m => m.Id == id);
        if (job == null)
        {
            return NotFound();
        }

        return View(job);
    }

    // POST: JOBS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var job = await _context.Job.FindAsync(id);
        if (job != null)
        {
            _context.Job.Remove(job);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool JobExists(int? id)
    {
        return _context.Job.Any(e => e.Id == id);
    }
}
