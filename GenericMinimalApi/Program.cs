using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(GroupActionsByPath);
builder.Services.AddDbContext<Db>(UserInMemoryDatabase);

var app = builder.Build();
app.UseSwagger().UseSwaggerUI();
app.AddModel<BookEnitty, BookDto, AddBookDto>("books");
app.AddModel<GameEnitty, GameDto, AddGameDto>("games");
app.Run();

void GroupActionsByPath(SwaggerGenOptions options) => 
    options.TagActionsBy(apiDescription => new[]
    {
        !(apiDescription?.RelativePath is { } relativePath) ? string.Empty : relativePath.Contains('/') ? relativePath.Remove(relativePath.IndexOf('/')) : relativePath
    });

void UserInMemoryDatabase(DbContextOptionsBuilder options) => 
    options.UseInMemoryDatabase("Db");