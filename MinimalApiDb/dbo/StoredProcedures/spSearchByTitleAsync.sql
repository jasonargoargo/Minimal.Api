CREATE PROCEDURE [dbo].[spSearchByTitleAsync]
	@SearchTerm nvarchar(50)
AS
begin

SELECT Isbn, Title, Author, ShortDescription, [PageCount], ReleaseDate 
FROM Books 
WHERE Title LIKE '%' + @SearchTerm + '%';

end
