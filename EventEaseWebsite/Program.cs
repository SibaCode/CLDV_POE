using EventEase.Data;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using Microsoft.Extensions.Configuration;
using EventEase.Services; 
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBlobService, AzureBlobService>();
builder.Services.AddScoped<IBlobService, AzureBlobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");

app.Run();
