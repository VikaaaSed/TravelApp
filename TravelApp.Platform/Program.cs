using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;
using TravelApp.Platform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<CityHttpClient>();
builder.Services.AddHttpClient<CityViewHttpClient>();
builder.Services.AddHttpClient<LocationViewHttpClient>();
builder.Services.AddHttpClient<LocationGalleryHttpClient>();
builder.Services.AddHttpClient<FeedbackViewHttpClient>();
builder.Services.AddHttpClient<FeedbackHttpClient>();
builder.Services.AddHttpClient<UserHttpClient>();

builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IClientIpService, ClientIpService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

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
