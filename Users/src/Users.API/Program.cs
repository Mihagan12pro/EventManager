var builder = WebApplication.CreateBuilder(args);


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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var apiGroup = app.MapGroup("auth/api");
apiGroup.MapGet("/login", () => "Products list");
apiGroup.MapGet("/register", () => "Products list");

app.Run();
