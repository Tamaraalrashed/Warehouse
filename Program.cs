using Data;
using Microsoft.EntityFrameworkCore;
using REST;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Data.JwtSettings>(builder.Configuration);

// Configure services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
    options.AddPolicy("AllowAllOrigins",
    builder => builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Unit of Work
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<JwtSettings>();

var app = builder.Build();

// Apply pending migrations on startup
app.UseSqliteMigration();

// Configure route prefix and base path
var apiBasePath = "/Api/v1";
app.UseMiddleware<GlobalRoutePrefixMiddleware>(apiBasePath);
app.UsePathBase(new PathString(apiBasePath));

app.ConfigureExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseRouting();
app.MapControllers();
app.Run();

