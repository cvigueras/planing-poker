using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Repositories;
using System.Configuration;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace PlaningPoker.Api.Startup;

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
        services.AddScoped<IVoteRepository, VoteRepository>();
        DataBase.Create();
        services.AddCors(
            x =>
            {
                x.AddPolicy("AllowClientPolicy", options =>
                {
                    options.WithOrigins(GetPolicyUrlSettings())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();

                    options.WithOrigins(GetPolicyUrlSettings())
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

    private string GetPolicyUrlSettings() => this.ConfigRoot.GetSection("Enviroments").Get<Enviroments>().ClientPolicyUrl;
}