using EntityFrameworkImpIementação.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configura o entity framework para usar o banco de dados em memoria
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseInMemoryDatabase("EntityFrameworkImpIementacaoDb");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
