using Microsoft.EntityFrameworkCore;
using Salat.Data;
using Salat.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

// Добавляем DbContext
builder.Services.AddDbContext<SalatDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default") ??
                     "Server=(localdb)\\MSSQLLocalDB;Database=SalatDb_New1;Trusted_Connection=True;"));

// Добавляем контроллеры
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.WriteIndented = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS нужно добавлять до Build()
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware CORS
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Сидинг данных
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalatDbContext>();

    if (!db.FoodItems.Any())
    {
        db.FoodItems.AddRange(
            new FoodItem { Name = "Kartul", ProteinPct = 2.0, FatPct = 0.1, CarbsPct = 17.0 },
            new FoodItem { Name = "Hapukoor 20%", ProteinPct = 2.5, FatPct = 20.0, CarbsPct = 3.0 },
            new FoodItem { Name = "Vorst", ProteinPct = 12.0, FatPct = 25.0, CarbsPct = 1.5 }
        );
        db.SaveChanges();
    }
}

app.Run();
