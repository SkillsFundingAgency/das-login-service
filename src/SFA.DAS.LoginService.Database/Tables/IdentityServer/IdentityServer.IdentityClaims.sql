CREATE TABLE [IdentityServer].[IdentityClaims]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[IdentityResourceId] [int] NOT NULL,
	CONSTRAINT [PK_IdentityClaims] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [IdentityServer].[IdentityClaims] ADD CONSTRAINT [FK_IdentityClaims_IdentityResources_IdentityResourceId] FOREIGN KEY([IdentityResourceId])
REFERENCES [IdentityServer].[IdentityResources] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [IdentityServer].[IdentityClaims] CHECK CONSTRAINT [FK_IdentityClaims_IdentityResources_IdentityResourceId]
GO