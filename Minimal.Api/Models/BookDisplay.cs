namespace Minimal.Api.Models
{
    public class BookDisplay
    {
        public string Isbn { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public int PageCount { get; set; }
        public string ReleaseDate { get; set; } = default!;
    }
}
