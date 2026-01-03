using ECommerceProjesi.DataAccess;
using Microsoft.EntityFrameworkCore;
using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.Business.Concrete;
using Microsoft.AspNetCore.Identity;
using ECommerceProjesi.WebUI.IdentitySeed;
using System.Diagnostics;
using ECommerceProjesi.WebUI.Utils;
using Microsoft.AspNetCore.Identity.UI.Services; 
using ECommerceProjesi.WebUI.Services; 

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<ECommerceContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ECommerceContext>()
    .AddDefaultTokenProviders();
    builder.Services.AddTransient<IEmailSender, EmailSender>();

    builder.Services.AddRazorPages();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    });
    builder.Services.AddScoped<IUrunService, UrunManager>();
    builder.Services.AddScoped<IKategoriService, KategoriManager>();
    builder.Services.AddScoped<IMarkaService, MarkaManager>();
    builder.Services.AddScoped<IUrunVaryantiService, UrunVaryantiManager>();
    builder.Services.AddScoped<IUrunResmiService, UrunResmiManager>();
    builder.Services.AddScoped<IMusteriService, MusteriManager>();
    builder.Services.AddScoped<ISepetService, SepetManager>();
    builder.Services.AddScoped<IAdresService, AdresManager>();
    builder.Services.AddScoped<ISiparisService, SiparisManager>();
    builder.Services.AddTransient<IEmailSender, EmailSender>();

    builder.Services.AddTransient<FileHelper>(); 
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapAreaControllerRoute(
        name: "AdminArea",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();

    await SeedIdentity.EnsurePopulatedAsync(app);

    Debug.WriteLine(">>> Uygulama Başarıyla Başlatıldı (app.Run() öncesi) <<<");
    app.Run();
}
catch (Exception ex)
{
    Debug.WriteLine("##### UYGULAMA BAŞLATILIRKEN ÇÖKTÜ! #####");
    Debug.WriteLine($"HATA MESAJI: {ex.Message}");
    if (ex.InnerException != null)
    {
        Debug.WriteLine($"İÇ HATA (Inner Exception): {ex.InnerException.Message}");
    }
    Debug.WriteLine($"STACK TRACE:\n{ex.StackTrace}");
    Debug.WriteLine("########################################");
}
