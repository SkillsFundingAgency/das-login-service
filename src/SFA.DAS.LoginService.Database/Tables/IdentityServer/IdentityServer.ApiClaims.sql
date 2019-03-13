CREATE TABLE [IdentityServer].[ApiClaims]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
	CONSTRAINT [PK_ApiClaims] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [IdentityServer].[ApiClaims] ADD CONSTRAINT [FK_ApiClaims_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [IdentityServer].[ApiResources] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [IdentityServer].[ApiClaims] CHECK CONSTRAINT [FK_ApiClaims_ApiResources_ApiResourceId]
GO