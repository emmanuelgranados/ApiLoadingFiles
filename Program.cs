using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("*")
            .WithHeaders("*")
            .WithMethods("*");
        }
    );
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext < apiOperadores.Data.AztecaGeneralContext > (
    option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("aztecaGralDB"));

    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
