using Microsoft.EntityFrameworkCore;

partial class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options) { }
}