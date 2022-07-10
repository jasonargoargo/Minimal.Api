CREATE PROCEDURE [dbo].[spUpdateAsync]
	@Isbn nvarchar(50),
	@Title nvarchar(50),
	@Author nvarchar(100),
	@ShortDescription nvarchar(500),
	@PageCount int,
	@ReleaseDate nvarchar(50)
AS
begin

UPDATE Books 
SET Isbn = @Isbn, Title = @Title, Author = @Author, ShortDescription = @ShortDescription, [PageCount] = @PageCount, ReleaseDate = @ReleaseDate
WHERE Isbn = @Isbn;

end
