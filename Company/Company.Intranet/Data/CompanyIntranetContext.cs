using Microsoft.EntityFrameworkCore;

namespace Company.Intranet.Data
{
    public class CompanyIntranetContext : DbContext
    {
        public CompanyIntranetContext (DbContextOptions<CompanyIntranetContext> options)
            : base(options)
        {
        }

        public DbSet<Company.Intranet.Models.CMS.Page> Page { get; set; } = default!;
        public DbSet<Company.Intranet.Models.CMS.News> News { get; set; } = default!;
    }
}
