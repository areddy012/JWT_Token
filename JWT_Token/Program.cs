using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Hotjar", builder =>
    {
        builder.WithOrigins("https://static.hotjar.com")
            .WithMethods("GET", "POST")
            .WithHeaders("Content-Type");

        builder.WithOrigins("https://static.hotjar.com")
            .WithMethods("GET", "PUT", "DELETE")
            .WithHeaders("Authorization", "Content-Type");
    });
});
// Add services to the container.

builder.Services.AddControllers();
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

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
