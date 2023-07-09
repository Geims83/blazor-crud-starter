using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCRUD.Data.Models;
using BlazorCRUD.Services;
using Dapper;

namespace BlazorCRUD.Data
{
    public class ItemRepo : IRepository<Item>
    {
        private readonly IDbFactory _dbf;

        public ItemRepo()
        {
        }

        public ItemRepo(IDbFactory dbFactory)
        {
            _dbf = dbFactory;
        }

        private const string _Insert = """
            INSERT dbo.Item ([Description], [Value], CategoryId)
            OUTPUT inserted.ItemId, inserted.[Description], inserted.CategoryId
            VALUES (@Description, @Value, @CategoryId)
            """;

        public async Task<Item> AddAsync(Item entity)
        {
            using var db = _dbf.GetConnection();

            var item = await db.QueryAsync<Item, Category, Item>(
                _Insert,
                (i, c) => {
                    i.Category = c;
                    return i;
                },
                new {
                    Description = entity.Description,
                    Value = entity.Value,
                    CategoryId = entity.Category.CategoryId
                },
                splitOn: "CategoryId");
            
            return item.FirstOrDefault();
        }

        private const string _Delete = """
            DELETE i
            OUTPUT deleted.ItemId, deleted.[Description], deleted.[Value], deleted.CategoryId, c.[Description]
            FROM dbo.Item AS i
            INNER JOIN dbo.Category AS c ON c.CategoryId = i.CategoryId
            WHERE i.ItemId = @id
            """;

        public async Task<Item> DeleteAsync(long id)
        {
            using var db = _dbf.GetConnection();
            var item = await db.QueryAsync<Item, Category, Item>(
                _Delete,
                (i, c) => {
                    i.Category = c;
                    return i;
                },
                new { 
                    id = id
                },
                splitOn: "CategoryId");
            
            return item.FirstOrDefault();
        }


        private const string _SelectSingle = """
            SELECT
                i.ItemId,
                i.[Description],
                i.[Value],
                i.CategoryId,
                c.Description
            FROM dbo.Item AS i
            INNER JOIN dbo.Category AS c ON c.CategoryId = i.CategoryId
            WHERE
                ItemId = @id
            """;

        public async Task<Item> GetByIdAsync(long id)
        {
            using var db = _dbf.GetConnection();

            var list = await db.QueryAsync<Item, Category, Item>(
                _SelectSingle,
                (i, c) => {
                    i.Category = c;
                    return i;
                },
                new {
                    id = id
                },
                splitOn: "CategoryId");

            return list.FirstOrDefault();
        }


        private const string _Update = """
            UPDATE dbo.Item 
            SET [Description] = @Description, [Value] = @Value, CategoryId = @CategoryId
            OUTPUT inserted.ItemId, inserted.[Description], inserted.CategoryId
            WHERE ItemId = @ItemId
            """;

        public async Task<Item> UpdateAsync(Item entity)
        {
            using var db = _dbf.GetConnection();
            var list = await db.QueryAsync<Item, Category, Item>(_Update, (i, c) => {
                i.Category = c;
                return i;
            },
            new {
                Description = entity.Description,
                Value = entity.Value,
                CategoryId = entity.Category.CategoryId,
                ItemId = entity.ItemId
            },
            splitOn: "CategoryId");

            return list.FirstOrDefault();
        }

        private const string _SelectAll = """
            SELECT
                ItemId,
                i.[Description],
                [Value],
                i.CategoryId,
                c.Description
            FROM dbo.Item AS i
            INNER JOIN dbo.Category AS c ON c.CategoryId = i.CategoryId
            """;

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            using var db = _dbf.GetConnection();
            return await db.QueryAsync<Item, Category, Item>(
                _SelectAll,
                (i, c) => {
                    i.Category = c;
                    return i;
                },
                splitOn: "CategoryId");
        }

        private const string _SelectWhereDescription = """
            SELECT
                ItemId,
                i.[Description],
                [Value],
                i.CategoryId,
                c.Description
            FROM dbo.Item AS i
            INNER JOIN dbo.Category AS c ON c.CategoryId = i.CategoryId
            WHERE i.[Description] LIKE @filter
            """;
        public async Task<IEnumerable<Item>> FilterByDescriptionAsync(string filter)
        {
            using var db = _dbf.GetConnection();
            return await db.QueryAsync<Item, Category, Item>(
                _SelectWhereDescription,
                (i, c) =>
                {
                    i.Category = c;
                    return i;
                },
                new { filter = $"%{filter}%" },
                splitOn: "CategoryId");
        }
    }
}
