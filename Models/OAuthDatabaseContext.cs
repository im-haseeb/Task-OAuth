using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace OAuth.Models
{
	public class OAuthDatabaseContext : DbContext
	{
		public OAuthDatabaseContext(DbContextOptions<OAuthDatabaseContext> options)
		: base(options)
		{
		}

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

		}
	}
}
