using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tournament.Api.Extensions;
using Tournament.Core.Repositories;
using Tournament.Data.Data;
using Tournament.Data.Repositories;
using Tournament.Presentation;

namespace Tournament.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TournamentApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentApiContext") ?? throw new InvalidOperationException("Connection string 'TournamentApiContext' not found.")));

            // Add services to the container.
            builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IUoW, UoW>();
            builder.Services.AddProblemDetails();
            builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddXmlDataContractSerializerFormatters();

            // Configure logging
            builder.Logging.ClearProviders(); // Optional: Clears default providers
            builder.Logging.AddConsole(); // Add Console logging
            builder.Logging.AddDebug(); // Add Debug logging

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(TournamentMappings));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
