CREATE TABLE [LoginService].[InvalidPasswords](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
	[Password] [nvarchar](250) NOT NULL 
) ON [PRIMARY] 
GO

CREATE UNIQUE CLUSTERED INDEX [IX_InvalidPasswords_Password] ON [LoginService].[InvalidPasswords] (Password)
