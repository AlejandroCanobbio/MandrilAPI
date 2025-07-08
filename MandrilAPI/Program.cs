using MandrilAPI.Helpers;
using MandrilAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddScoped<MandrilService>();
builder.Services.AddScoped<HabilidadService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var mandrilService = scope.ServiceProvider.GetRequiredService<MandrilService>();
    var habilidadService = scope.ServiceProvider.GetRequiredService<HabilidadService>();
    //await mandrilService.CargarMandrilesDePrueba();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();