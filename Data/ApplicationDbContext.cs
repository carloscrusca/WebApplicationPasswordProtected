using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplicationPasswordProtected.Models;

namespace WebApplicationPasswordProtected.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationProfileUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WebApplicationPasswordProtected.Models.Todo> Todo { get; set; }
        public DbSet<WebApplicationPasswordProtected.Models.FileModel> File { get; set; }
    }
}
