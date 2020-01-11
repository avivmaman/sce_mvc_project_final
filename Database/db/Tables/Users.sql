CREATE TABLE [dbo].[Users]
(
	[StudentID] INT NOT NULL , 
    [Password] NVARCHAR(50) NOT NULL, 
    [FullName] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [Rank] INT NULL DEFAULT 1, 
    [Id] INT NOT NULL PRIMARY KEY IDENTITY
)
