CREATE TABLE [IdentityServer].[ApiScopeClaims]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[ApiScopeId] [int] NOT NULL,
	CONSTRAINT [PK_ApiScopeClaims] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [IdentityServer].[ApiScopeClaims] ADD CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ApiScopeId] FOREIGN KEY([ApiScopeId])
REFERENCES [IdentityServer].[ApiScopes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [IdentityServer].[ApiScopeClaims] CHECK CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ApiScopeId]
GO