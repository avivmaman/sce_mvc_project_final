CREATE TABLE [dbo].[Grades]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CourseID] INT NOT NULL, 
    [StudentID] INT NOT NULL, 
    [aGrade] INT NULL, 
    [bGrade] INT NULL
)
