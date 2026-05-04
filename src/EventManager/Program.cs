using EventManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();

builder.Host.ConfigureLogging(opt =>
{
    opt.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);
    opt.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);
});

builder.Services.AddSwaggerGen(options =>
{
    var binDirectory = new DirectoryInfo(AppContext.BaseDirectory);
    var files = binDirectory.GetFiles("*.xml");

    foreach (var file in files)
    {
        options.IncludeXmlComments(file.FullName);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCustomMiddleware();

app.MapControllers();

app.Run();