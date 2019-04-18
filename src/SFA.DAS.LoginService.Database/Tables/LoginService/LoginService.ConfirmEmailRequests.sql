CREATE TABLE [LoginService].[ConfirmEmailRequests]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[ValidUntil] [datetime2](7) NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[RequestedDate] [datetime2](7) NOT NULL,
	[IdentityToken] [nvarchar](MAX) NOT NULL,
) ON [PRIMARY]
