using DesafioEstacionamentoBenner.Repositories;
using DesafioEstacionamentoBenner.Repositories.Interfaces;
using DesafioEstacionamentoBenner.Services;
using DesafioEstacionamentoBenner.Services.Interfaces;
using Infrastructure.DataBase;
using Infrastructure.Middleware;
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

builder.Services.AddTransient<IParkingService, ParkingService>();
builder.Services.AddTransient<IPriceListService, PriceListService>();

builder.Services.AddTransient<IParkingRepository, ParkingRepository>();
builder.Services.AddTransient<IPriceListRepository, PriceListRepository>();

// Este bloco de código é condicionalmente compilado com base na definição da constante de compilação 'TESTE'.
// Se a constante de compilação 'TESTE' estiver definida, o bloco dentro de '#if TESTE' será incluído na compilação.
// Caso contrário, o bloco dentro de '#else' será executado.
#if TESTE
// Adiciona um contexto de banco de dados usando o provedor de banco de dados SQL Server.
// Configura o contexto com a cadeia de conexão "DatabaseTest" e define opções específicas do SQL Server.
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseTest"),
    sqlServerOptionsAction: sqlOptions =>
    {
        // Habilita tentativas de reexecução em caso de falhas na conexão.
        // Define um máximo de 5 tentativas e um atraso máximo de 30 segundos entre elas.
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));
#else
    // Adiciona um contexto de banco de dados usando o provedor de banco de dados SQL Server.
    // Configura o contexto com a cadeia de conexão "Database" e define opções específicas do SQL Server.
    builder.Services.AddDbContext<Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database"),
        sqlServerOptionsAction: sqlOptions =>
        {
            // Habilita tentativas de reexecução em caso de falhas na conexão.
            // Define um máximo de 5 tentativas e um atraso máximo de 30 segundos entre elas.
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

app.UseMiddleware<ErrorHandlerMiddleware>();

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