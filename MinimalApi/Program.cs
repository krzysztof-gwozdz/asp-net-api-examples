using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Db>(options => options.UseInMemoryDatabase("Db"));

var app = builder.Build();
app.UseSwagger().UseSwaggerUI();

app.MapGet("/items", async (Db db, int? page, int? pageSize) =>
{
    page ??= 1;
    pageSize ??= 2;
    var skip = pageSize.Value * (page.Value - 1);
    var take = pageSize.Value * page.Value;
    var items = (await db.Items.Skip(skip).Take(take).ToListAsync());
    return Results.Ok(items.Select(item => new ItemDto(item.Id, item.Name, item.Price)));
});
app.MapGet("/items/{id}", async (Db db, Guid id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null)
        return Results.NotFound();
    return Results.Ok(new ItemDto(item.Id, item.Name, item.Price));
});
app.MapPost("/items", async (Db db, AddItemDto dto) =>
{
    var id = Guid.NewGuid();
    db.Items.Add(new Item(id, dto.Name, dto.Price));
    await db.SaveChangesAsync();
    return Results.Created(id.ToString(), null);
});
app.MapPut("/items/{id}", async (Db db, Guid id, AddItemDto dto) =>
{
    var item = await db.Items.AsNoTracking().FirstOrDefaultAsync(_ => _.Id == id);
    if (item is null)
        db.Items.Add(new Item(id, dto.Name, dto.Price));
    else
        db.Items.Update(new Item(id, dto.Name, dto.Price));
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.MapDelete("/items/{id}", async (Db db, Guid id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null)
        return Results.NotFound();
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.Run();

partial class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options) { }

    public DbSet<Item> Items => Set<Item>();
}

record ItemDto(Guid Id, string Name, double Price);
record AddItemDto(string Name, double Price);
record Item(Guid Id, string Name, double Price);