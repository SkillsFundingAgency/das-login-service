CREATE TABLE [dbo].[Invitations](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[GivenName] [nvarchar](250) NOT NULL,
	[FamilyName] [nvarchar](250) NOT NULL,
	[SourceId] [nvarchar](250) NOT NULL,
	[Code] [nvarchar](250) NOT NULL,
	[ValidUntil] [datetime2](7) NOT NULL,
	[CallbackUri] [nvarchar](250) NOT NULL,
	[UserRedirectUri] [nvarchar](250) NOT NULL,
	[CodeConfirmed] [bit] NOT NULL DEFAULT 0,
	[IsUserCreated] [bit] NOT NULL DEFAULT 0,
	[IsCalledBack] [bit] NOT NULL DEFAULT 0,
	[CallbackDate] [datetime2](7) NULL,
	[Id] [uniqueidentifier] NOT NULL,
) ON [PRIMARY]
GO


