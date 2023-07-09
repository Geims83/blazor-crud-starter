CREATE TABLE [dbo].[Category] (
    [CategoryId] BIGINT IDENTITY (1,1) NOT NULL,
    [Description] NVARCHAR(MAX),
    CONSTRAINT [PK_Category] PRIMARY KEY ([CategoryId])
)