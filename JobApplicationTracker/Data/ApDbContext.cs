using JobApplicationTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data
{
    public class ApDbContext : IdentityDbContext<Users>
    {
        public ApDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
