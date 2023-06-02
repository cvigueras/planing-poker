using System.Data.SQLite;
using webapi.Cards.Repositories;
using webapi.Games.Repositories;
using webapi.Helpers;
using webapi.Users.Repositories;

namespace webapi.Startup;

public class Startup
{
    public IConfiguration ConfigRoot
    {
        get;
    }

    public Startup(IConfiguration configuration)
    {
        ConfigRoot = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped(_ => new SQLiteConnection("Data Source=./PlaningPoker.db"));
        services.AddAutoMapper(typeof(MapperConfig));
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGuidGenerator, GuidGenerator>();
        services.AddScoped<ICardRepository, CardRepository>();
        DataBase.Create();
        services.AddCors(
            x =>
            {
                x.AddPolicy("AllowClientPolicy", options =>
                {
                    options.WithOrigins("https://localhost:5002")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();

                    options.WithOrigins("http://localhost:5002")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        services.AddSignalR();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseRouting();
        app.MapControllers();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<PlaningHub>("/hubs/planing");
        });

        app.Run();
    }
}