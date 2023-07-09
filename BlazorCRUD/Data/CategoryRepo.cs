using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCRUD.Data.Models;
using BlazorCRUD.Services;
using Dapper;

namespace BlazorCRUD.Data
{
    public class CategoryRepo : IRepository<Category>
    {
        private readonly IDbFactory _dbf;

        public CategoryRepo()
        {
        }

        public CategoryRepo(IDbFactory dbFactory)
        {
            _dbf = dbFactory;
        }

        private const string _Insert = """
            INSERT dbo.Category ([Description])
            OUTPUT inserted.CategoryId, inserted.[Description]
            VALUES (@Description)
            """;

        public async Task<Category> AddAsync(Category entity)
        {
            using var db = _dbf.GetConnection();
            
            return await db.QuerySingleAsync<Category>(_Insert, entity);
        }


        private const string _Delete = """
            DELETE c 
            OUTPUT deleted.CategoryId, deleted.[Description]
            FROM dbo.Category AS c
            WHERE c.CategoryId = @id
            """;
        public async Task<Category> DeleteAsync(long id)
        {
            using var db = _dbf.GetConnection();
            return await db.QuerySingleAsync<Category>(_Delete, new { id = id });
        }

        private const string _SelectWhereDescription = """
            SELECT
                CategoryId,
                [Description]
            FROM dbo.Category
            WHERE [Description] LIKE @filter
            """;
        public async Task<IEnumerable<Category>> FilterByDescriptionAsync(string filter)
        {
            using var db = _dbf.GetConnection();
            return await db.QueryAsync<Category>(_SelectWhereDescription, new { filter = $"%{filter}%" });
        }

        private const string _SelectSingle = """
            SELECT
                CategoryId,
                [Description]
            FROM dbo.Category
            WHERE
                CategoryId = @id
            """;
        public async Task<Category> GetByIdAsync(long id)
        {
            using var db = _dbf.GetConnection();
            return await db.QuerySingleOrDefaultAsync<Category>(_SelectSingle,  new { id = id } );
        }

        private const string _Update = """
            UPDATE dbo.Category 
            SET [Description] = @Description
            OUTPUT inserted.CategoryId, inserted.[Description]
            WHERE CategoryId = @CategoryId
            """;
        public async Task<Category> UpdateAsync(Category entity)
        {
            using var db = _dbf.GetConnection();            
            return await db.QuerySingleAsync<Category>(_Update, entity); 
        }

        private const string _SelectAll = """
            SELECT
                CategoryId,
                [Description]
            FROM dbo.Category
            """;
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var db = _dbf.GetConnection();
            return await db.QueryAsync<Category>(_SelectAll);
        }

        
        private const string _SelectItems = """
            SELECT
                ItemId,
                [Description],
                [Value],
                CategoryId
            FROM dbo.Item
            WHERE CategoryId = @id
            """;
        public async Task<IEnumerable<Item>> GetItemsAsync(long categoryId)
        {
            using var db = _dbf.GetConnection();
            return await db.QueryAsync<Item>(
                _SelectItems,
                new { id =  categoryId});
        }
    }
}
