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
      var decks = new DeckRepository();
      decks.Add(new DefaultDeck());
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
              builder.AllowAnyMethod().AllowAnyHeader()
                     .WithOrigins("http://localhost:44365")
                     .AllowCredentials();
            }));
      services.AddSignalR();
      services.AddSingleton<DeckService>();
      services.AddSingleton(typeof(RoomService));
      services.AddSingleton<RoundService>();
      services.AddSingleton<UserService>();
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
  }
}