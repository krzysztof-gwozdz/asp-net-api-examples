using Microsoft.EntityFrameworkCore;

partial class Db
{
    public DbSet<BookEnitty> Books => Set<BookEnitty>();
}

record BookEnitty(Guid Id, string Name, double? Price, string? Author) : Entity<BookDto>(Id)
{
    public override BookDto ToDto() => new(Id, Name, Price, Author);
}

record BookDto(Guid Id, string Name, double? Price, string? Author)
{
}

record AddBookDto(string Name, double? Price, string? Author) : AddDto<BookEnitty>
{
    public override BookEnitty ToEntity(Guid? id = default) => new(id ?? Guid.NewGuid(), Name, Price, Author);
}