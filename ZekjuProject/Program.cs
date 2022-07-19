using System.Diagnostics;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;
using Service.Model;
using Service.Services;

namespace ZekjuProject;

class Program
{
    private static IConfigurationRoot _configurationRoot;


    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();
                configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configurationRoot = configuration.Build();
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services
                    .AddDataContextServices(_configurationRoot)
                    .AddServices();
            });
    }

    static async Task Main(string[] args)
    {
        //args = new string[] { "2018-01-01", "2018-01-15", "1" };
        var host = CreateHostBuilder(args).Build();
        var errors = Validation(args);

        if (errors.Any())
        {
            errors.ForEach(error => Console.WriteLine(error));
            Console.ReadLine();
            return;
        }

        DateTime.TryParse(args[0], out var startDate);
        DateTime.TryParse(args[1], out var endDate);
        int.TryParse(args[2], out var agencyId);

        var request = new RequestDto(startDate, endDate, agencyId);
        var calculator = host.Services.GetService<IDataCalculator>();

        var watch = new Stopwatch();
        watch.Start();
        var data = await calculator.GetFlights(request, default);
        watch.Stop();

        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        await host.RunAsync();
    }

    private static List<string> Validation(string[] args)
    {
        List<string> errors = new List<string>();

        if (args.Length != 3)
            errors.Add("The number of parameters is inappropriate");

        return errors;
    }
}