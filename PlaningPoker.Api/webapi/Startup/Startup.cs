using System.Data.SQLite;

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
        services.AddScoped(_ => new SQLiteConnection("Data Source=./Game.db"));
        services.AddAutoMapper(typeof(MapperConfig));
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddCors(
            x =>
            {
                x.AddPolicy("AllowClientPolicy", options =>
                {
                    options.WithOrigins("https://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();

                    options.WithOrigins("http://localhost:8080")
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