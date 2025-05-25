//var builder = WebApplication.CreateBuilder(args);
//
//// Add services to the container.
//
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//
//var app = builder.Build();
//
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//
//app.UseHttpsRedirection();
//
//app.UseAuthorization();
//
//app.MapControllers();
//
//app.Run();
//

using landlord_be;
using Microsoft.AspNetCore;

namespace Todo2Api
{
    public class Program
    {
         public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5001")
                .Build();
    }
}