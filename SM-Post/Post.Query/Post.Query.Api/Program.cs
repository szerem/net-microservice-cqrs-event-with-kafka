using Confluent.Kafka;
using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Action<DbContextOptionsBuilder> configureDbContext = ( o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
Action<DbContextOptionsBuilder> configureDbContext = ( o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory> (new DatabaseContextFactory(configureDbContext));

// Create database and tables form code 
var dataContext = builder.Services.BuildServiceProvider().GetService<DatabaseContext>();
dataContext!.Database.EnsureCreated();


builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<EventJsonConverter>(x => new EventJsonConverter());
builder.Services.AddScoped<IEventConsumer, EventConsumer>();


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
