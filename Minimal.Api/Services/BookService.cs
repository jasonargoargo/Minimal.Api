using Dapper;
using Minimal.Api.Data;
using Minimal.Api.Models;

namespace Minimal.Api.Services
{
    public class BookService : IBookService
    {
        private readonly IDataAccess dataAccess;

        public BookService(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<bool> CreateAsync(Book book)
        {
            var existingBook = await GetByIsbnAsync(book.Isbn);
            if (existingBook is null)
            {
                return false;
            }

            var parms = new
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                ShortDescription = book.ShortDescription,
                PageCount = book.PageCount,
                ReleaseDate = book.ReleaseDate
            };
            var dynParms = new DynamicParameters(parms);
            dynParms.Add("@Id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            await dataAccess.SaveData("dbo.spCreateAsync", dynParms);
            book.Id = dynParms.Get<int>("Id");

            return true;
        }

        public async Task<bool> DeleteAsync(string isbn)
        {
            await dataAccess.SaveData("dbo.spUpdateAsync", new {Isbn = isbn});
            return true;
        }

        public async Task<IEnumerable<BookDisplay>> GetAllAsync()
        {
            return await dataAccess.LoadAllData<BookDisplay, dynamic>("dbo.spGetAllAsync", new { });
        }

        public async Task<BookDisplay?> GetByIsbnAsync(string isbn)
        {
            return await dataAccess.LoadData<BookDisplay, dynamic>("dbo.spGetByIsbnAsync", new { Isbn = isbn });
        }

        public async Task<IEnumerable<BookDisplay>> SearchByTitleAsync(string searchTerm)
        {
            return await dataAccess.LoadAllData<BookDisplay, dynamic>("dbo.spSearchByTitleAsync", new { SearchTerm = searchTerm });
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var existingBook = await GetByIsbnAsync(book.Isbn);
            if(existingBook is null)
            {
                return false;
            }

            var parms = new BookDisplay
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                ShortDescription = book.ShortDescription,
                PageCount = book.PageCount,
                ReleaseDate = book.ReleaseDate
            };

            await dataAccess.SaveData("dbo.spUpdateAsync", parms);
            return true;
        }
    }
}
