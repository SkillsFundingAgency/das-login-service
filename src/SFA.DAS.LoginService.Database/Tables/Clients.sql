CREATE TABLE [LoginService].[Clients](
	[Id] [uniqueidentifier] NOT NULL,
	[ServiceDetails] [nvarchar](MAX) NOT NULL,
	[IdentityServerClientId] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO


