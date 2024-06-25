using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using PresentationLayer.Helper;
using Stripe;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<ApplicationDBContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection(nameof(Stripe)));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options
                =>options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromDays(4))
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDBContext>();
            //builder.Services.AddScoped(typeof(IGenericRepository<Product>), typeof(GenericRepository<Product>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new CategoryProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile()));
            builder.Services.AddAutoMapper(C=>C.AddProfile(new CartProfile()));
            builder.Services.AddSingleton<IEmailSender, EmailSender>();



            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();



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
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secret key").Get<string>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area=Admin}/{controller=Categories}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }
}