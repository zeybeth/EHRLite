using EHRLite.Data;
using EHRLite.Repository; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity (Güvenlik) Ayarları
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Yetkisiz girişlerde yönlendirilecek sayfaları belirtiyoruz
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";       // Giriş yapmamışsa buraya at
    options.LogoutPath = "/Identity/Account/Logout";     // Çıkış yapınca buraya git
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Yetkisi yetmiyorsa buraya at
});

// 3. Identity Sayfalarının (Login/Register) çalışması için gerekli servis
builder.Services.AddRazorPages();

// Email Sender Servis Kaydı
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EHRLite.Utility.EmailSender>();

// 4. Repository Servislerinin Kaydı
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IVisitRepository, VisitRepository>();
builder.Services.AddScoped<ILabResultRepository, LabResultRepository>();

// 5. MVC Servisi
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Pipeline Ayarları ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 6. Güvenlik Sıralaması (Önce Kimlik Doğrulama, Sonra Yetki)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 7. Identity Sayfalarının Yönlendirmesi
app.MapRazorPages();

app.Run();