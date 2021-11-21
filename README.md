# ASP.NET API examples

A few projects that represents usage of asp.net framework for creating API. 

## Minimal API
A simple API that use [minimal apis](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0) added in 6.0 version of ASP.NET Core. 

### Features
- Gets items from database + pagination.
- Gets an item by id.
- Adds an item.
- Idempotent adds or updates an item.
- Deletes an item.
- Saves and gets items to SQL in memory database.

## Generic minimal API
Evolution of **Minimal API** that allows to create API based on registered models.

### Features for each registered model
- Gets models from database + pagination.
- Gets a model by id.
- Adds a model.
- Idempotent adds or updates a model.
- Deletes a model.
- Saves and gets models to SQL in memory database.

### How to add another model?
#### A simple, not very educational way:
1. Copy and paste `./Models/Books.cs` file.
2. Rename pasted file.
3. Rename usage of word *Book* in this file.
4. Change entity, dto and adddto file based on your requirements.
5. Call `app.AddModel<TEnitty, TDto, TAddDto>(string modelPath);` in `./Program.cs` before `app.Run()`.\
Of course, you can copy and paste line for *books* model…

#### Correct way - *this is the way*:
1. Create an implementation of `EntityDto`.
2. Create an implementation of `Entity<T>(Guid id)`, where T is reference type from the previous point.
3. Create an implementation of `AddDto<T>(Guid id)`, where T is reference type from the previous point.
4. Add `DbSet<T>` to `Db` class, where T is reference type from second point.
5. Call `app.AddModel<TEnitty, TDto, TAddDto>(string modelPath);` in `./Program.cs` before `app.Run()`, where:
   - `TEnitty` is reference type from third point.
   - `TDto` is reference type from the first point.
   - `TAddDto` is reference type from the second point.
   - `modelPath` is relative path to model used for API routing.


## License
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)