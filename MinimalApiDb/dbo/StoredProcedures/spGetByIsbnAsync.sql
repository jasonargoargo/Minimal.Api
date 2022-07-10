CREATE PROCEDURE [dbo].[spGetByIsbnAsync]
	@Isbn nvarchar(50)

AS
begin

SELECT Isbn, Title, Author, ShortDescription, [PageCount], ReleaseDate 
FROM Books 
WHERE Isbn = @Isbn;

end