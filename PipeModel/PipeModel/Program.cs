using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeModel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var collector = new FakeMetricsCollector();
            new HostBuilder()
                .ConfigureHostConfiguration(builder => builder.AddCommandLine(args))
                .ConfigureAppConfiguration((context,builder)=>builder
                    .AddJsonFile(path:"appsettings.json",optional:false)
                    .AddJsonFile(path:$"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                                optional:true))
                .ConfigureServices((context,svcs) => svcs
                //.AddHostedService<PerformanceMetricsCollector>() 与下面语句完全等效
                    .AddSingleton<IProcessorMetricsCollector>(collector)
                    .AddSingleton<IMemoryMetricsCollector>(collector)
                    .AddSingleton<INetworkMetricsCollector>(collector)
                    .AddSingleton<IMetricsDeliverer, FakeMetricsDeliverer>()
                    .AddSingleton<IHostedService, PerformanceMetricsCollector>()
                
                    .AddOptions()
                    .Configure<MetricsCollectionOptions>(
                    context.Configuration.GetSection("MetricsCollection")))
                .ConfigureLogging(builder=>builder.AddConsole())
                .Build().Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
