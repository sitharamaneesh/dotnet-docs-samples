var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// ...

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.Use((context, next) =>
    {
        if (context.Request.Scheme != "https")
        {
            var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            context.Response.Redirect(httpsUrl);
            return Task.CompletedTask;
        }

        return next();
    });
}

// ...




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


CancellationTokenSource cancellation = new();
app.Lifetime.ApplicationStopping.Register( () =>
{
    cancellation.Cancel();
});

app.MapGet("/Environment", () =>
{
    return new EnvironmentInfo();
});

// This API demonstrates how to use task cancellation
// to support graceful container shutdown via SIGTERM.
// The method itself is an example and not useful.
app.MapGet("/Delay/{value}", async (int value) =>
{
    try
    {
        await Task.Delay(value, cancellation.Token);
    }
    catch(TaskCanceledException)
    {
    }
    
    return new {Delay = value};
});

app.Run();
