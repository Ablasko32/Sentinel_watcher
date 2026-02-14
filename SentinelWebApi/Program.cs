using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SentinelCore.DAL;
using SentinelWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SentinelWatcher API",
        Version = "v1"
    });
});

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularFe",  policy =>
    {
        policy.WithOrigins("http://localhost:4200") //TEMP TODO
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

//DB
builder.Services.AddDbContext<SentinelContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Identity and cookies
builder.ConfigureIdentity();

//repositories
builder.ConfigureRepositoryLayer();
//services
builder.ConfigureServiceLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed roles and admin user and migrate database if pending migrations exist
await app.InitializeDbAsync();

app.UseCors("AngularFe");

app.Run();