using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OAuth.Models;
using OAuth.Repositories;
using OAuth.Seeders;
using OAuth.Services;

namespace OAuth
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<OAuthDatabaseContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "OAuth API", Version = "v1" });
            });

            builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IUserRepo, UserRepo>();


            var app = builder.Build();

			using var scope = app.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<OAuthDatabaseContext>();

            dbContext.Database.Migrate();
            // Create default roles
            SeedUsers.CreateDefaultUsers(dbContext).Wait();


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "OAuth API V1");
			});


			app.UseRouting();

			app.UseAuthorization();

			app.UseAuthentication();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
