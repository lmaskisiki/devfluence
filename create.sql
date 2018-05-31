
drop table [dbo].[AgentExecution]
 CREATE TABLE [dbo].[AgentExecution]
(
	[ExecutionId] INT NOT NULL IDENTITY(1,1),  
    [Command] NVARCHAR(50) NULL, 
    [Result] NVARCHAR(50) NULL, 
    [ExecutionTime] DATETIME NULL
	
)

insert into AgentExecution
(Command,Result,ExecutionTime) Values('ip','193.192.2.194','2018-12-31 23:59:59');

insert into AgentExecution
(Command,Result,ExecutionTime) Values('hostname','DevFluence7','2018-01-01 21:59:59');
 
