using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseHttpsRedirection();

app.MapGet("/", async context =>
{
    var aspnetVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?.FrameworkName;
    var serverTime = DateTime.UtcNow;

    var responseMessage = $"<div style='text-align:center;'>" +
                          $"<h1>Hello, World!</h1>" +
                          $"<p>ASP.NET Version: {aspnetVersion}</p>" +
                          $"<p>Server Time: {serverTime}</p>" +
                          $"</div>";

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(responseMessage);
});

app.Run();
