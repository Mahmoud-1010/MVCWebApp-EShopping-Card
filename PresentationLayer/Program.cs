using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Mapper;
using Microsoft.AspNetCore.Identity;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddRazorPages()
            //                .AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDBContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddScoped(typeof(IGenericRepository<Product>), typeof(GenericRepository<Product>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new CategoryProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile()));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //      name: "default",
            //    pattern: "{area=Admin}/{controller=Categories}/{action=Index}/{id?}"
            //    );
            //    endpoints.MapControllerRoute(
            //      name: "Customer",
            //    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
            //    );
            //    endpoints.MapControllerRoute(
            //      name: "Customers",
            //    pattern: "{area=Customers}/{controller=Home}/{action=Index}/{id?}"
            //    );
            //});
            app.MapControllerRoute(
                name: "Customer",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "Customers",
                pattern: "{area=Customers}/{controller=Home}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Categories}/{action=Index}/{id?}"
                );
            
            app.Run();
        }
    }
}