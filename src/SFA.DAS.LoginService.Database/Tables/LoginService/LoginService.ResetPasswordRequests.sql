CREATE TABLE [LoginService].[ResetPasswordRequests]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[Code] [nvarchar](250) NOT NULL,
	[ValidUntil] [datetime2](7) NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[RequestedDate] [datetime2](7) NOT NULL,
	[IdentityToken] [nvarchar](250) NOT NULL,
) ON [PRIMARY]
