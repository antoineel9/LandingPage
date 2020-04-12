using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NWI2.Data;
using WebPWrecover.Services;


namespace NWI2
{
    public class Startup
    {
       public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string CookiePath { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.


        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            })
            .AddCookie(options => new CookieAuthenticationOptions
            {
                //AuthenticationScheme = "Cookies", // Removed in 2.0
                ExpireTimeSpan = TimeSpan.FromHours(12),
                SlidingExpiration = false,
                Cookie = new CookieBuilder
                {
                    Path = CookiePath,
                    Name = "MyCookie"
                }
            }).//AddOpenIdConnect(options => GetOpenIdConnectOptions());
            Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI()


                   //  options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    ;
                
                // ; AddMicrosoftAccount(microsoftOptions => { })
                // ; AddGoogle(googleOptions => { })
                // ; AddTwitter(twitterOptions => { })
                //; AddFacebook(facebookOptions => { });

                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddRazorPagesOptions(options =>
                    {
                        // options.Conventions.AuthorizeFolder("/Profile);
                    }

                    );

                //requires
                // using Microsoft.AspNetCore.Identity.UI.Services;
                // using WebPWrecover.Services;
                services.AddTransient<IEmailSender, EmailSender>();
                services.Configure<AuthMessageSenderOptions>(Configuration);

                services.AddRazorPages();
            }
        }

        private void AddEntityFrameworkStores<T>()
        {
            throw new NotImplementedException();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}