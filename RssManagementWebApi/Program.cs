using RssManagementWebApi.DB;
using RssManagementWebApi.Services.Implementations;
using RssManagementWebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


using var dbContext = new ApplicationDbContext();
dbContext.Database.EnsureCreated();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IFeedsService, FeedsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();