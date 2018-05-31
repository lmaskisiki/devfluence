CREATE TABLE [dbo].[Table]
(
	[ExecutionId] INT NOT NULL PRIMARY KEY, 
    [Command] NVARCHAR(50) NULL, 
    [Result] NVARCHAR(50) NULL, 
    [ExecutionTime] TIMESTAMP NULL
)
