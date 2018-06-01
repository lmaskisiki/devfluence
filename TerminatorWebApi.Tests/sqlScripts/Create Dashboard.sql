USE [SkyNetPortalDB]
GO

/****** Object: Table [dbo].[Dashboard] Script Date: 2018/05/31 15:58:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
Drop Table [dbo].[Dashboard]
CREATE TABLE [dbo].[Dashboard] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Target]        NVARCHAR (50) NULL,
    [Action]       NVARCHAR (50) NULL,
    [ActionResult]  NVARCHAR (50) NULL,
    [ExecutionTime] DATETIME      NULL
);

Insert into [dbo].[Dashboard] ([Target],[Action],[ActionResult], [ExecutionTime]) Values('Dashboard','Add Agent','Added Agent 1', '2018-12-31 23:59:59');

select * from [dbo].[Dashboard]


