using System.Globalization;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Services.AddSerilog((_, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

    // Add services to the container.
    builder.Services.AddRazorPages();

    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception exception)
{
    var path = $".logs/start-host-log-{DateTime.UtcNow.Date.ToString("ddMMyyyy")}.txt";
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File(path)
        .CreateLogger();
    Log.Logger.Error(exception, "Unhandled exception");
}