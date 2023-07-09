CREATE TABLE [dbo].[Item] (
    [ItemId] BIGINT IDENTITY(1, 1) NOT NULL,
    [Description] NVARCHAR (MAX),
    [Value] DECIMAL(18,2),
    [CategoryId] BIGINT NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY ([ItemId]),
    CONSTRAINT [FK_Item_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category]([CategoryId])
)