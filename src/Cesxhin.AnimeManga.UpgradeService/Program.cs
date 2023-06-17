using Cesxhin.AnimeManga.Application.CheckManager;
using Cesxhin.AnimeManga.Application.CheckManager.Interfaces;
using Cesxhin.AnimeManga.Modules.CronJob;
using Cesxhin.AnimeManga.Modules.Generic;
using Cesxhin.AnimeManga.Modules.Schema;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;
using System;

namespace Cesxhin.AnimeManga.UpgradeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SchemaControl.Check();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //rabbit
                    services.AddMassTransit(
                    x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(
                                Environment.GetEnvironmentVariable("ADDRESS_RABBIT") ?? "localhost",
                                "/",
                                credentials =>
                                {
                                    credentials.Username(Environment.GetEnvironmentVariable("USERNAME_RABBIT") ?? "guest");
                                    credentials.Password(Environment.GetEnvironmentVariable("PASSWORD_RABBIT") ?? "guest");
                                });
                        });
                    });

                    //setup nlog
                    var level = Environment.GetEnvironmentVariable("LOG_LEVEL").ToLower() ?? "info";
                    LogLevel logLevel = NLogManager.GetLevel(level);
                    NLogManager.Configure(logLevel);

                    //select service between anime or manga
                    var serviceSelect = Environment.GetEnvironmentVariable("SELECT_SERVICE") ?? "video";

                    //cronjob for check health
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();
                        q.ScheduleJob<HealthJob>(trigger => trigger
                            .StartNow()
                            .WithDailyTimeIntervalSchedule(x => x.WithIntervalInSeconds(60)), job => job.WithIdentity("upgrade-"+serviceSelect));
                        //q.AddJob<CronJob>(job => job.WithIdentity("update"));
                    });
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

                    

                    if (serviceSelect.ToLower().Contains("video"))
                        services.AddTransient<IUpgrade, UpgradeVideo>();
                    else
                        services.AddTransient<IUpgrade, UpgradeBook>();

                    services.AddHostedService<Worker>();
                });
    }
}
