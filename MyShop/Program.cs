using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain;
using MyShop.Domain.Repositories.EntityFramework;
using MyShop.Domain.Repositories.Interfaces;
using MyShop.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ICarInfoRepository, EFCarInfoRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("AppDBContext")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
	opts.User.RequireUniqueEmail = true;
	opts.Password.RequiredLength = 6;
	opts.Password.RequireNonAlphanumeric = false;
	opts.Password.RequireLowercase = false;
	opts.Password.RequireUppercase = false;
	opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//настраиваем authentication cookie
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.Name = ".AspNetCore.AuthCookie";
	options.Cookie.HttpOnly = true;
	options.LoginPath = "/account/login";
	options.AccessDeniedPath = "/account/accessdenied";
	options.SlidingExpiration = true;
});


//настраиваем политику авторизации для Admin area
builder.Services.AddAuthorization(x =>
{
	x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.Cookie.Name = ".AspNetCore.Session";
	options.IdleTimeout = TimeSpan.FromMinutes(20);
	options.Cookie.IsEssential = true;
	options.Cookie.HttpOnly = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMvc(x => { x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea")); x.EnableEndpointRouting = false; });

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();


app.UseMvc(routes =>
{
	routes.MapRoute("admin", "{area:exists}/{controller = Home}/{action=Index}/{id?}");
	routes.MapRoute(
		name: "default",
		template: "{controller=Home}/{action=Index}/{id?}"
		);
});

app.Run();
