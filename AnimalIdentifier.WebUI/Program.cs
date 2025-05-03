using AnimalIdentifier.Application.Extentions;
using AnimalIdentifier.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

#region DependencyInjection
builder.Services.AddApplicationServices();
builder.Services.AddApplicationServices(builder.Configuration);
#endregion

builder.Services.AddControllers();

builder.Services.AddHttpClient("AnimalApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:7287");
});

builder.Services.AddAuthentication(options =>
{
    // تنظیمات اولیه احراز هویت
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(cookie =>
{
    // تنظیمات کوکی
    cookie.Cookie.Name = "keycloak.cookie";
    cookie.Cookie.MaxAge = TimeSpan.FromMinutes(60);
    cookie.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    cookie.SlidingExpiration = true;
})
.AddOpenIdConnect(options =>
{
    // تنظیمات OpenID Connect برای ارتباط با Keycloak
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = builder.Configuration.GetSection("Keycloak")["ServerRealm"];
    options.ClientId = builder.Configuration.GetSection("Keycloak")["ClientId"];
    options.ClientSecret = builder.Configuration.GetSection("Keycloak")["ClientSecret"];
    options.MetadataAddress = builder.Configuration.GetSection("Keycloak")["Metadata"];
    options.RequireHttpsMetadata = false;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.SaveTokens = true;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.NonceCookie.SameSite = SameSiteMode.Unspecified;
    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = ClaimTypes.Role,
        ValidateIssuer = true,
        ValidateLifetime = true
    };
    options.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization(options =>
{
    // تعریف سیاست برای نقش "admin"
    options.AddPolicy("admin", policy =>
        policy.RequireClaim(ClaimTypes.Role, "admin"));

    //// تعریف سیاست برای نقش "reader"
    //options.AddPolicy("reader", policy =>
    //    policy.RequireClaim(ClaimTypes.Role, "reader"));

    //// سیاست برای دسترسی به هر دو نقش "admin" و "reader"
    //options.AddPolicy("adminOrReader", policy =>
    //    policy.RequireAssertion(context =>
    //        context.User.HasClaim(c =>
    //            (c.Value == "admin") || (c.Value == "reader"))));
});

var app = builder.Build();

// تنظیمات درخواست‌ها
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();