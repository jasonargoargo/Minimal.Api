CREATE PROCEDURE [dbo].[spDeleteAsync]
	@Isbn nvarchar(50)
AS
begin

DELETE FROM Books WHERE Isbn = @Isbn;

end
