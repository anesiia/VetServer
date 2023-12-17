using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using VetServer.Data;
using VetServer.Models.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VetCareDbContext>(options =>
    options.UseSqlServer("Server=Anesia;Database=VetCare_db;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddDbContext<VetCareDbContext>(options =>
    options.UseSqlServer("Server=Anesia;Database=VetCare_db;Trusted_Connection=True;TrustServerCertificate=True;")
           .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddScoped<IPasswordHasher<Owners>, PasswordHasher<Owners>>();
builder.Services.AddScoped<IPasswordHasher<Doctors>, PasswordHasher<Doctors>>();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("CultureInvariant");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();



// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctor}/{action=Login}/{id?}");

app.Run();