using Microsoft.AspNetCore.Cors;
using BusTicketAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 📌 MSSQL bağlantısını ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 📌 CORS Politikasını Tanımla
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular'ın çalıştığı URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// 📌 Controller servisini ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 📌 CORS'u Kullan
app.UseCors("AllowAngularOrigins");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
