using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotNetToolbox.Sophia.WebApp.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
}
