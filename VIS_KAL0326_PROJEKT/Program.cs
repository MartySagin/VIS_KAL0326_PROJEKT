using DataAccess.Interfaces;
using DataAccess.Repositories;
using DataAccess;
using Application.Interfaces;
using Application.BusinessLogic;

namespace VIS_KAL0326_PROJEKT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Singletons
            builder.Services.AddSingleton<IDatabaseAccess>(new DatabaseAccess(connectionString));
            builder.Services.AddSingleton<ILoginCacheService>(new LoginCacheService(TimeSpan.FromMinutes(30)));
            builder.Services.AddSingleton<IMyLogger, MyLogger>();

            // Repositories
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IReservationRepository, ReservationRepository>();
            builder.Services.AddTransient<IClubRepository, ClubRepository>();

            // Services
            builder.Services.AddTransient<ILoginService, LoginService>();
            builder.Services.AddTransient<IClubService, ClubService>();
            builder.Services.AddTransient<IReservationService, ReservationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
