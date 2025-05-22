var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia automaticamente os controllers
app.MapControllers();

// Redireciona a raiz "/" para a interface do Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();


