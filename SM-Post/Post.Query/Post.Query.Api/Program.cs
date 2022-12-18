using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Action<DbContextOptionsBuilder> configureDbContext = ( o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
Action<DbContextOptionsBuilder> configureDbContext = ( o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory> (new DatabaseContextFactory(configureDbContext));

// Create database and tables form code 
var dataContext = builder.Services.BuildServiceProvider().GetService<DatabaseContext>();
dataContext.Database.EnsureCreated();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
