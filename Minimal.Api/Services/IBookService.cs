using Minimal.Api.Models;

namespace Minimal.Api.Services
{
    public interface IBookService
    {
        public Task<bool> CreateAsync(Book book);
        public Task<BookDisplay?> GetByIsbnAsync(string isbn);
        public Task<IEnumerable<BookDisplay>> GetAllAsync();
        public Task<IEnumerable<BookDisplay>> SearchByTitleAsync(string searchTerm);
        public Task<bool> UpdateAsync(Book book);
        public Task<bool> DeleteAsync(string isbn);
    }
}
