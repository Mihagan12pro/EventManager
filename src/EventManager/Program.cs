using EventManager;
using EventManager.Middleware;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScopedDependencies();

//builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Manager API V1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Event Manager API V2", Version = "v2" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Manager API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Event Manager API V2");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<CustomExceptionMiddleware>();

app.MapControllers();

app.Run();