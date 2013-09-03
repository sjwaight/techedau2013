CREATE TABLE [YOUR_DATABASE].[UserDeviceRegistration] (
    [id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [ServiceType] NVARCHAR (MAX) NULL,
    [ServiceKey]  NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);