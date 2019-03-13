CREATE TABLE [IdentityServer].[ClientProperties]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[ClientId] [int] NOT NULL,
	CONSTRAINT [PK_ClientProperties] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [IdentityServer].[ClientProperties] ADD CONSTRAINT [FK_ClientProperties_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [IdentityServer].[Clients] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [IdentityServer].[ClientProperties] CHECK CONSTRAINT [FK_ClientProperties_Clients_ClientId]
GO