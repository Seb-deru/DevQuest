using DevQuest.Application;
using DevQuest.Infrastructure;

namespace DevQuest.HttpApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services from each layer
        builder.Services.AddWebApi();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
