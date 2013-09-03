CREATE TABLE [YOUR_DATABASE].[UserTask] (
    [id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [name]     NVARCHAR (MAX) NULL,
    [notes]    NVARCHAR (MAX) NULL,
    [done]     BIT            NULL,
    [assignee] NVARCHAR (450) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [assignee]
    ON [YOUR_DATABASE].[UserTask]([assignee] ASC);