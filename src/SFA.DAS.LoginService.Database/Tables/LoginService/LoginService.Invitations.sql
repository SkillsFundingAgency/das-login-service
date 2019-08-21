CREATE TABLE [LoginService].[Invitations]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[GivenName] [nvarchar](250) NOT NULL,
	[FamilyName] [nvarchar](250) NOT NULL,
	[SourceId] [nvarchar](250) NOT NULL,
	[ValidUntil] [datetime2](7) NOT NULL,
	[CallbackUri] [nvarchar](250) NOT NULL,
	[UserRedirectUri] [nvarchar](250) NOT NULL,
	[IsUserCreated] [bit] NOT NULL DEFAULT 0,
	[IsCalledBack] [bit] NOT NULL DEFAULT 0,
	[CallbackDate] [datetime2](7) NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[Inviter] [nvarchar](500) NULL,
    [InviterId] [nvarchar](250) NULL,
) ON [PRIMARY]
