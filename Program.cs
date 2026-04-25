
using IPValidatorAssignment.Repositories;
using IPValidatorAssignment.Services;

namespace IPValidatorAssignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddSingleton<IBlockedCountryService, BlockedCountryService>();
            builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
            {
                client.BaseAddress = new Uri("https://api.ipgeolocation.io/");
            });

            builder.Services.AddHostedService<TemporalBlockCleanupService>();
            builder.Services.AddSingleton<IBlockCountryRepository, BlockCountryRepository>();
            builder.Services.AddSingleton<ILogService, LogService>();


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
        }
    }
}
