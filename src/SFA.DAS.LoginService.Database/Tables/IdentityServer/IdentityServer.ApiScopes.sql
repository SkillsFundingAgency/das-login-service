CREATE TABLE [IdentityServer].[ApiScopes]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Description] [nvarchar](1000) NULL,
	[Required] [bit] NOT NULL,
	[Emphasize] [bit] NOT NULL,
	[ShowInDiscoveryDocument] [bit] NOT NULL,
	[ApiResourceId] [int] NOT NULL,
	CONSTRAINT [PK_ApiScopes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [IdentityServer].[ApiScopes] ADD CONSTRAINT [FK_ApiScopes_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [IdentityServer].[ApiResources] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [IdentityServer].[ApiScopes] CHECK CONSTRAINT [FK_ApiScopes_ApiResources_ApiResourceId]
GO