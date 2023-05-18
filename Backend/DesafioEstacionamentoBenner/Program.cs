using DesafioEstacionamentoBenner.Repositories;
using DesafioEstacionamentoBenner.Repositories.Interfaces;
using DesafioEstacionamentoBenner.Services;
using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy("MyPolicy", policy =>
{
    policy.WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injection Dependencies Services
builder.Services.AddTransient<IParkingService, ParkingService>();
builder.Services.AddTransient<IPriceListService, PriceListService>();

// Injection Dependencies Repositories
builder.Services.AddTransient<IParkingRepository, ParkingRepository>();
builder.Services.AddTransient<IPriceListRepository, PriceListRepository>();

#if TESTE
builder.Services.AddDbContext<Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseTest"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));
#else
builder.Services.AddDbContext<Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));
#endif

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    Context context = scope.ServiceProvider.GetRequiredService<Context>();
#if !TESTE
    context.Database.EnsureCreated();
#elif TESTE
    if (!context.Database.EnsureCreated())
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
#endif
}

app.UseCors("MyPolicy");

using (IServiceScope scope = app.Services.CreateScope())
{
    Context context = scope.ServiceProvider.GetRequiredService<Context>();

    context.Database.EnsureCreated();
}


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