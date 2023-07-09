using System;
using BlazorCRUD.Data;
using BlazorCRUD.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Fast.Components.FluentUI;


namespace BlazorCRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddTransient<IDbFactory, DbFactory>((db) =>
            {
                return new DbFactory(connectionString);
            });

            builder.Services.AddTransient<CategoryRepo, CategoryRepo>();
            builder.Services.AddTransient<ItemRepo, ItemRepo>();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddHttpClient();

            builder.Services.AddFluentUIComponents(options =>
            {
                options.HostingModel = BlazorHostingModel.Server;
                options.IconConfiguration = ConfigurationGenerator.GetIconConfiguration();
                options.EmojiConfiguration = ConfigurationGenerator.GetEmojiConfiguration();
            });

            WebApplication? app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}