CREATE TABLE [dbo].[Books]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Isbn] NVARCHAR(50) NOT NULL, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Author] NVARCHAR(100) NOT NULL, 
    [ShortDescription] NVARCHAR(500) NOT NULL, 
    [PageCount] INT NOT NULL, 
    [ReleaseDate] DATETIME2 NOT NULL
)
