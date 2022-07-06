CREATE PROCEDURE [dbo].[spCreateAsync]
	@Isbn nvarchar(50),
	@Title nvarchar(50),
	@Author nvarchar(100),
	@ShortDescription nvarchar(500),
	@PageCount int,
	@ReleaseDate nvarchar(50),
	@Id INT OUTPUT
AS
begin
	set nocount on;

	insert into dbo.Books([Isbn], [Title], [Author], [ShortDescription], [PageCount], [ReleaseDate])
	values (@Isbn, @Title, @Author, @ShortDescription, @PageCount, @ReleaseDate)

	SET @Id = SCOPE_IDENTITY();
end