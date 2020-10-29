using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using CoreDropDownList.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CoreDropDownList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CoreDropDownList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddControllersWithViews();
            //services.AddRazorPages();

            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DPMdbConnection"),
                     sqlServerOptionsAction: SqlOptions =>
                     {
                         SqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                     }));

            services.AddRazorPages();
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                //TokenProvider
                //options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                ///
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            // ***Default DataProtectionTokenProvider
            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(5));
            //End

            //// cookie 
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });

            services.AddDistributedMemoryCache();

            services.AddSession();

            //add MVC Policy
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

            }).AddXmlSerializerFormatters()
          .AddRazorRuntimeCompilation();

            ///services.AddMvc()
          //  .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            //End Claim Policy

            //Ploicy start 

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddAuthorization(option =>
            {

                option.AddPolicy("SuperAccountantPolicy", policy =>
                            policy.RequireAssertion(context => AuthorizeAccess(context)));

                option.AddPolicy("SuperAccountantPolicy", policy =>
                        policy.RequireAssertion(context =>
                            context.User.IsInRole("SuperAccountant")));


                option.AddPolicy("SrAccountantPolicy", policy =>
                            policy.RequireAssertion(context =>
                                context.User.IsInRole("SuperAccountant")
                                || context.User.IsInRole("SrAccountant")));

                option.AddPolicy("JrAccountantPolicy", policy =>
                                policy.RequireAssertion(context =>
                                    context.User.IsInRole("SuperAccountant")
                                    || context.User.IsInRole("SrAccountant")
                                    || context.User.IsInRole("JrAccountant")));

                //option.AddPolicy("StudentAccountantPolicy", policy =>
                //                policy.RequireAssertion(context =>
                //                    context.User.IsInRole("SuperAccountant")
                //                    || context.User.IsInRole("SrAccountant")
                //                    || context.User.IsInRole("JrAccountant")));

                //option.AddPolicy("SrExamAccountantPolicy", policy =>
                //            policy.RequireAssertion(context =>
                //                context.User.IsInRole("SuperAccountant")
                //                || context.User.IsInRole("SrAccountant")));

            });



            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN-MOONGLADE";
                options.FormFieldName = "CSRF-TOKEN-MOONGLADE-FORM";
            });

            //// login problem
            services.AddControllers().AddRazorRuntimeCompilation();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddRazorPages().AddRazorRuntimeCompilation();



        }

        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("SrAccountant") &&
                      context.User.HasClaim(claim => claim.Type == "Create" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "ListUsers" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "EditUserInRole" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "EditRole" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "ManageUserClaim" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "Edit" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "Delete" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "Index" && claim.Value == "true") ||
                      context.User.IsInRole("SuperAccountant") &&
                      context.User.HasClaim(claim => claim.Type == "Create" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "ListUsers" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "EditUserInRole" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "EditRole" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "ManageUserClaim" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "Edit" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "Delete" && claim.Value == "true") ||
                      context.User.HasClaim(claim => claim.Type == "Index" && claim.Value == "true");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
            TableSeeder.Initialize(context, userManager, roleManager).Wait();
        }

    }
}
