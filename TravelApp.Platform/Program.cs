using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;
using TravelApp.Platform.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using TravelApp.Platform.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;
using TravelApp.Platform.Areas.Admin.Services;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

if (env.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

if (string.IsNullOrWhiteSpace(jwtSettings?.Key))
    throw new Exception("JWT ���� �� ������� ��������");

// ���������� ������������ ��� JwtSettings �� ��������� ��������
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// ���������� �������� � ��������������
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<CityHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<CityViewHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<FeedbackHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<FeedbackViewHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<LocationGalleryHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<LocationHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<LocationViewHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<UserHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<FavoriteLocationHttpClient>(ConfigureHttpClient);
builder.Services.AddHttpClient<UserFollowerHttpClient>(ConfigureHttpClient);
void ConfigureHttpClient(IServiceProvider sp, HttpClient client)
{
    var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
}

// ���������� �������������� � JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt_token"))
            {
                context.Token = context.Request.Cookies["jwt_token"];
            }
            return Task.CompletedTask;
        }
    };
});

// ���������� HTTP-��������
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<CityHttpClient>();
builder.Services.AddHttpClient<CityViewHttpClient>();
builder.Services.AddHttpClient<LocationViewHttpClient>();
builder.Services.AddHttpClient<LocationGalleryHttpClient>();
builder.Services.AddHttpClient<FeedbackViewHttpClient>();
builder.Services.AddHttpClient<FeedbackHttpClient>();
builder.Services.AddHttpClient<UserHttpClient>();
builder.Services.AddHttpClient<LocationHttpClient>();
builder.Services.AddHttpClient<FavoriteLocationHttpClient>();

// ����������� ��������
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IClientIpService, ClientIpService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAdminCityService, AdminCityService>();
builder.Services.AddScoped<IAdminLocationService, AdminLocationService>();
builder.Services.AddScoped<IAdminFeedbackService, AdminFeedbackService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<INotificationService, TempDataNotificationService>();
builder.Services.AddScoped<IFavoriteLocationService, FavoriteLocationService>();
builder.Services.AddScoped<IUserFollowerService, UserFollowerService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IFeedBackUser, FeedBackService>();
builder.Services.AddScoped<ILocationByUser, LocationService>();
builder.Services.AddScoped<IUserSearch, UserService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
