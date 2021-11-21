using Microsoft.EntityFrameworkCore;

partial class Db
{
    public DbSet<GameEnitty> Games => Set<GameEnitty>();
}

record GameEnitty(Guid Id, string Name, double? Price, string? Designer) : Entity<GameDto>(Id)
{
    public override GameDto ToDto() => new(Id, Name, Price, Designer);
}

record GameDto(Guid Id, string Name, double? Price, string? Designer)
{
}

record AddGameDto(string Name, double? Price, string? Designer) : AddDto<GameEnitty>
{
    public override GameEnitty ToEntity(Guid? id = default) => new(id ?? Guid.NewGuid(), Name, Price, Designer);
}