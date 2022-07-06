using Minimal.Api.Models;

namespace Minimal.Api.Services
{
    public interface IBookService
    {
        public Task CreateAsync(Book book);
        public Task<Book?> GetByIsbnAsync(string isbn);
        public Task<IEnumerable<Book>> GetAllAsync();
        public Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm);
        public Task UpdateAsync(Book book);
        public Task DeleteAsync(string isbn);
    }
}
