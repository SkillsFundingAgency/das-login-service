CREATE TABLE [LoginService].[UserLogs](
	[Id] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[Action] [nvarchar](250) NOT NULL,
	[Result] [nvarchar](250) NOT NULL,
	[ExtraData] [nvarchar](MAX) NOT NULL
) ON [PRIMARY]
GO