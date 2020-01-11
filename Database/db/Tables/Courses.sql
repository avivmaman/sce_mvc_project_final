CREATE TABLE [dbo].[Courses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [teacher] INT NOT NULL, 
    [startTime] INT NOT NULL, 
    [duration] INT NOT NULL, 
    [name] NVARCHAR(50) NOT NULL, 
    [day] INT NOT NULL, 
    [aDate] DATETIME NOT NULL, 
    [bDate] DATETIME NOT NULL, 
    [classroom] NVARCHAR(50) NULL, 
    [classrooma] NVARCHAR(50) NULL, 
    [classroomb] NVARCHAR(50) NULL
)
