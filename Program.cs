using HtmlAgilityPack;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using StonkBot.StonkBot;
using StonkBot.StonkBot._SBResources;
using StonkBot.StonkBot.Actions;
using StonkBot.StonkBot.Database;
using StonkBot.StonkBot.Services.TDAmeritrade;
using StonkBot.StonkBot.Services.WebScrape;
using StonkBot.StonkBot.Utilities;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options => { options.ServiceName = "StonkBot-Service"; })
    .ConfigureServices(services =>
    {
        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);
        services.AddDbContext<IStonkBotDb, StonkBotDbContext>(ServiceLifetime.Transient);
        services.AddTransient<ConsoleWriter>();
        services.AddTransient<IStonkBotScheduler, StonkBotScheduler>();
        services.AddTransient<IStonkBotActions, StonkBotActions>();
        services.AddTransient<ISpecialActions, SpecialActions>();
        services.AddTransient<ITdaApiService, TdaApiService>();
        services.AddTransient<IUrlBuilder, UrlBuilder>();
        services.AddTransient<IWebullIpoListingFactory, WebullIpoListingFactory>();
        services.AddTransient<IWebullIpoScraperService, WebullIpoScraperService>();
        services.AddTransient<HtmlDocument>();
        services.AddTransient<HttpClient>();
        services.AddHostedService<WindowsBackgroundService>();
    })
    .ConfigureLogging((context, logging) => { logging.AddConfiguration( context.Configuration.GetSection("Logging")); })
    .Build();

await host.RunAsync();