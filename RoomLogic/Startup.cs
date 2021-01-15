using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RoomApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
      this.InitDefaultDeck();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddCors(options => options.AddPolicy(
        "CorsPolicy",
        builder =>
        {
           builder.AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials();
        }));
      services.AddSignalR();
      services.AddSingleton<ApplicationContext>();
      services.AddSingleton<DeckRepository>();
      services.AddSingleton<RoomRepository>();
      services.AddSingleton<RoundRepository>();
      services.AddSingleton<UserRepository>();

      services.AddSingleton<DeckService>();
      services.AddSingleton<RoomService>();
      services.AddSingleton<RoundService>();
      services.AddSingleton<UserService>();
      services.AddSingleton<RoundTimerService>();
      services.AddSingleton<UsersReadinessService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDefaultFiles();
        app.UseStaticFiles();
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseCors("CorsPolicy");
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<RoomHub>("/roomhub", option =>
        {
          option.Transports = HttpTransportType.WebSockets;
        });
      });
    }

    private void InitDefaultDeck()
    {
      var defaultDeck = new Deck("DefaultDeck");
      double[] numbers = { 0, 1.0 / 2, 1, 2, 3, 5, 8, 13, 20, 40, 100 };
      foreach (var number in numbers)
      {
        defaultDeck.AddCard(new Card(CardType.Valuable, number.ToString(), number));
      }

      defaultDeck.AddCard(new Card(CardType.Exceptional, "?", 0));
      defaultDeck.AddCard(new Card(CardType.Exceptional, "∞", 0));
      defaultDeck.AddCard(new Card(CardType.Exceptional, "☕", 0));
      var decks = new DeckRepository(new ApplicationContext());
      var deckService = new DeckService(decks);
      DeckService.DefaultDeck = decks.GetItem(decks.Add(defaultDeck));
    }
  }
}