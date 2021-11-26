using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MyApp.Infrastructure;

namespace MyApp.Server;

class Program
{

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        /*builder.Services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters.NameClaimType = "name";
            });*/


        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();


        // Connect to server with connection string
        builder.Services.AddDbContext<StudyBankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StudyBank")));
        builder.Services.AddScoped<IStudyBankContext, StudyBankContext>();
        //builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Seed();

        app.Run();
    }
}