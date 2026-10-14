
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.Models;
using JobApplicationTracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

[Authorize]
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
    public async Task<IActionResult> Index(string sortOrder, string searchString)    
    {
        //return View(await _context.Job.ToListAsync());

        var userId = _userManager.GetUserId(User);

        var jobs = _context.Job
            .Where(j => j.UserId == userId); //To only retreive the data based on the account.
                                             //.ToListAsync();


        if (!string.IsNullOrWhiteSpace(searchString))
        {
            jobs = jobs.Where(j => j.JobName.Contains(searchString));
        }
        ViewData["CurrentFilter"] = searchString;



        switch (sortOrder)
        {
            case "date_desc":
                jobs = jobs.OrderByDescending(j => j.DateApplied);
                break;

            case "date_asc":
                jobs = jobs.OrderBy(j => j.DateApplied);
                break;

            case "name_asc":
                jobs = jobs.OrderBy(j => j.JobName);
                break;

            case "name_desc":
                jobs = jobs.OrderByDescending(j => j.JobName);
                break;

            case "status_asc":
                jobs = jobs
                    .OrderBy(j => j.JobStatus)
                    .ThenByDescending(j => j.DateApplied);
                break;

            case "status_desc":
                jobs = jobs
                    .OrderByDescending(j => j.JobStatus)
                    .ThenByDescending(j => j.DateApplied);
                break;

            case "location_asc":
                jobs = jobs.OrderBy(j => j.Location);
                break;

            case "location_desc":
                jobs = jobs.OrderByDescending(j => j.Location);
                break;

            default:
                jobs = jobs.OrderByDescending(j => j.DateApplied);
                break;
        }

        ViewData["NameTitle"] = sortOrder switch
        {
            "name_asc" => "Role/Company ↑",
            "name_desc" => "Role/Company ↓",
            _ => "Role/Company"
        };

        ViewData["LocationTitle"] = sortOrder switch
        {
            "location_asc" => "Location Applied ↑",
            "location_desc" => "Location Applied ↓",
            _ => "Location Applied"
        };

        ViewData["StatusTitle"] = sortOrder switch
        {
            "status_asc" => "Status Applied ↑",
            "status_desc" => "Status Applied ↓",
            _ => "Status Applied"
        };

        ViewData["DateTitle"] = sortOrder switch
        {
            "date_asc" => "Date Applied ↑",
            "date_desc" => "Date Applied ↓",
            _ => "Date Applied"
        };


        ViewData["DateSort"] = sortOrder switch
        {
            null => "date_desc",
            "date_desc" => "date_asc",
            "date_asc" => "",
            _ => "date_desc"
        };



        ViewData["NameSort"] = sortOrder switch
        {
            "name_asc" => "name_desc",
            "name_desc" => "",
            _ => "name_asc"
        };

        ViewData["StatusSort"] = sortOrder switch
        {
            "status_asc" => "status_desc",
            "status_desc" => "",
            _ => "status_asc"
        };

        ViewData["LocationSort"] = sortOrder switch
        {
            "location_asc" => "location_desc",
            "location_desc" => "",
            _ => "location_asc"
        };


        return View(await jobs.ToListAsync());
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
    public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,JobName,JobDescription,JobStatus,Location,JobUrl,DateApplied,AdditionalNotes")] Job job)
    {
        if (id != job.Id)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);

        var existingJob = await _context.Job
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);

        if (existingJob == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            existingJob.JobName = job.JobName;
            existingJob.JobDescription = job.JobDescription;
            existingJob.JobStatus = job.JobStatus;
            existingJob.Location = job.Location;
            existingJob.JobUrl = job.JobUrl;
            existingJob.DateApplied = job.DateApplied;
            existingJob.AdditionalNotes = job.AdditionalNotes;

            await _context.SaveChangesAsync();

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
