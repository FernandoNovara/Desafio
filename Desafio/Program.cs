using Desafio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<DataContext>(
    options => options.UseMySql(
        Configuration["ConnectionString:DefaultConnection"],
        ServerVersion.AutoDetect(Configuration["ConnectionString:DefaultConnection"])
    )
); 

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);
    serverOptions.ListenAnyIP(5001,listenOptions => listenOptions.UseHttps());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
