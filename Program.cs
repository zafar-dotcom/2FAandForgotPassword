using IPMO.IServices;
using IPMO.Models.EmailService;
using IPMO.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var services=builder.Services;
// Add services to the container.
builder.Services.AddControllersWithViews();
services.AddScoped<IPupilRegistrationService, PupilRegistrationService>();
services.AddScoped<IAuth, Auth>();
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<IResetPassword,ResetPassword>();
services.AddScoped<IEncrypted, Encrypted>();
services.AddScoped<IPupilLogin,PupilLoginService>();
// For email configuration  for smtp 
var emailconfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfig);

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
