using API.Context;
using API.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace API
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

            //Add Service to the container
            builder.Services.AddScoped<DivisionRepository>();
            builder.Services.AddScoped<DepartmentRepository>();
            builder.Services.AddScoped<UserRepository>();

            // Register service to MyContext
            builder.Services.AddDbContext<MyContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

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