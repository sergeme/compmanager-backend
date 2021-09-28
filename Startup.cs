﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebApi.Helpers;
using WebApi.Middleware;
using WebApi.Services;

namespace WebApi
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // add services to the DI container
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<DataContext>();
      services.AddCors();
      services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddSwaggerGen();

      // configure strongly typed settings object
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      // configure DI for application services
      services.AddScoped<IAccountService, AccountService>();
      services.AddScoped<IEmailService, EmailService>();
    }

    // configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, DataContext context)
    {
      // migrate database changes on startup (includes initial db creation)
      context.Database.Migrate();

      app.UsePathBase("/api/v1");
      // generated swagger json and swagger ui middleware
      app.UseSwagger();
      app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Compmanager Backend"));

      app.UseRouting();

      // global cors policy
      app.UseCors(x => x
          .SetIsOriginAllowed(origin => true)
          .WithOrigins(Configuration.GetSection("Appsettings")["FrontendBaseUrl"])
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

      // global error handler
      app.UseMiddleware<ErrorHandlerMiddleware>();

      // custom jwt auth middleware
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(x => x.MapControllers());
    }
  }
}
