var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("signalr", builder =>
{
    builder
           .AllowAnyHeader()
           .AllowCredentials()
           .SetIsOriginAllowed(hostName => true);
}));

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IFileRepository, FileRepository>();
builder.Services.AddSingleton<IImageService, ImageService>();

var app = builder.Build();

// Let image service process the images before starting up the web app
IImageService service = app.Services.GetRequiredService<IImageService>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors("signalr");

app.Use((httpContext, function) =>
{
    var host = httpContext.Request.Host.ToString();
    // Simple IP filtering if needed
    if (isBadOrigin(host))
    {
        httpContext.Abort();
    }
    return function();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ImageHub>("/imagehub");
});

app.Run();

static bool isBadOrigin(string origin)
{
    // Do some checks on origin
    return false;
}