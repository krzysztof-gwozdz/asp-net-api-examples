using Microsoft.EntityFrameworkCore;

static class ModelsExtension
{
    public static WebApplication? AddModel<TEntitiy, TDto, TAddDto>(this WebApplication app, string modelPath)
        where TEntitiy : Entity<TDto>
        where TAddDto : AddDto<TEntitiy>
    {
        var url = $"/{modelPath}";
        app.MapGet(url, async (Db db, int? page, int? pageSize) =>
        {
            page ??= 1;
            pageSize ??= 2;
            var skip = pageSize.Value * (page.Value - 1);
            var take = pageSize.Value * page.Value;
            var models = (await db.Set<TEntitiy>().Skip(skip).Take(take).ToListAsync()).Select(entity => entity.ToDto());
            return Results.Ok(models);
        });
        app.MapGet(url + "/{id}", async (Db db, Guid id) =>
        {
            var entity = await db.Set<TEntitiy>().FindAsync(id);
            if (entity is null)
                return Results.NotFound();
            return Results.Ok(entity.ToDto());
        });
        app.MapPost(url, async (Db db, TAddDto dto) =>
        {
            var id = db.Set<TEntitiy>().Add(dto.ToEntity()).Entity.Id;
            await db.SaveChangesAsync();
            return Results.Created(id.ToString(), null);
        });
        app.MapPut(url + "/{id}", async (Db db, Guid id, TAddDto dto) =>
        {
            if (!db.Set<TEntitiy>().Any(_ => _.Id == id))
            {
                db.Set<TEntitiy>().Add(dto.ToEntity(id));
                await db.SaveChangesAsync();
                return Results.Created(id.ToString(), null);
            }
            else
            {
                db.Set<TEntitiy>().Update(dto.ToEntity(id));
                await db.SaveChangesAsync();
                return Results.Ok();
            }
        });
        app.MapDelete(url + "/{id}", async (Db db, Guid id) =>
        {
            var item = await db.Set<TEntitiy>().FindAsync(id);
            if (item is null)
                return Results.NotFound();
            db.Set<TEntitiy>().Remove(item);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
        return app;
    }
}

abstract record Entity<TDto>(Guid Id)
{
    public abstract TDto ToDto();
}

abstract record AddDto<TEntity>
{
    public abstract TEntity ToEntity(Guid? id = default);
}
