CREATE TABLE [dbo].[Tasks] (
    [Id]       INT         IDENTITY (1, 1) NOT NULL,
    [Name]     NCHAR (100) NULL,
    [Priority] SMALLINT    NULL,
    [StatusId] INT         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tasks_Statuses] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Statuses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_Name]
    ON [dbo].[Tasks]([Name] ASC);

