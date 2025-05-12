using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Repositories.Interfaces;
using TravelApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICityRepository, CityRepository>();

builder.Services.AddScoped<ICityInHomePageViewRepository, CityInHomePageViewRepository>();

builder.Services.AddScoped<ILocationInCityViewRepository, LocationInCityViewRepository>();

builder.Services.AddScoped<ILocationInHomePageViewRepository, LocationInHomePageViewRepository>();

builder.Services.AddScoped<ILocationGalleryRepository, LocationGalleryRepository>();

builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

builder.Services.AddScoped<IFeedbackViewRepository, FeedbackViewRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<IFavoriteLocationRepository, FavoriteLocationRepository>();

builder.Services.AddDbContextFactory<CityContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<CityInHomePageContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<LocationInCityContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<LocationGalleryContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<LocationInHomePageContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<FeedbackContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<FeedbackViewContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<UserContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<LocationContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddDbContextFactory<FavoriteLocationContext>(o => o.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
