using BookSellingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ApplicationUserContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationUserContextConnection' not found.");


builder.Services.AddDbContext<ApplicationUserContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddIdentity<AppUsers,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultUI().AddEntityFrameworkStores<ApplicationUserContext>();


builder.Services.AddAuthorization(o => {
    o.AddPolicy("readonlypolicy", builder => builder.RequireRole("Admin", "Clerk", "Manager", "User"));
    o.AddPolicy("writepolicy", builder => builder.RequireRole("Admin", "Clerk"));

});
// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
