using Cache.Interfaces;
using Cache.Services;
using Data;
using Domain.Interfaces;
using Domain.Services;
using Domain.Transactions;
using Domain.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            // Adds services for using Problem Details format
            builder.Services.AddProblemDetails();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddDbContext<ApplicationContext>((provider, options) =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                options
                    .UseNpgsql()
                    .UseLoggerFactory(loggerFactory);
            });
            builder.Services.AddScoped<IValidator<CreditTransaction>, CreditTransactionValidator>();
            builder.Services.AddScoped<IValidator<DebitTransaction>, DebitTransactionValidator>();
            builder.Services.AddScoped<IBalanceService, BalanceService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6380,password=password";
                options.InstanceName = "local";
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
