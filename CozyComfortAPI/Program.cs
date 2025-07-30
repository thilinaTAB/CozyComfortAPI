using Microsoft.EntityFrameworkCore;
using CozyComfortAPI.Data;
var builder = WebApplication.CreateBuilder(args);
var conn = "Data Source=HARSHAMAX_PC;Initial Catalog=CozyComfort;Integrated Security=True;TrustServerCertificate=True";
builder.Services.AddDbContext<AppDBContext>(op=>op.UseSqlServer(conn));
builder.Services.AddScoped<BlanketModelRepo>();
//Add services to the container
builder.Services.AddControllers();


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
