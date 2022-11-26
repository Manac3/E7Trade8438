using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//WindowsAuthentication
//string connectionString = "server=.\\SQLEXPRESS;database=ETrade8438;trusted_connection= true;multipleactiveresultsets=true;trustservercertificate=true;";

//burda her zaman derlenmesini engellemek için json dosyasýna attýk dolayýsýyla yourma aldýk ve app settingsden okumak için yeni komut vericez 


//Sql authentication
//string connectionString = "server=.\\SQLEXPRESS;database=ETrade8438;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true;";

string connectionString = builder.Configuration.GetConnectionString("ETradeDb");

#region IoC Container (Inversion of control Container)
builder.Services.AddDbContext<ETradeContext>(options => options.UseSqlServer(connectionString));
#endregion

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
