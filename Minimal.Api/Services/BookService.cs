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

        public async Task CreateAsync(Book book)
        {
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
        }

        public Task DeleteAsync(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetByIsbnAsync(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
