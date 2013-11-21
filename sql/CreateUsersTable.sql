USE [DealStealUnreal]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 11/21/2013 07:10:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Username] [nchar](10) NOT NULL,
	[Email] [nchar](50) NOT NULL,
	[Password] [nchar](50) NOT NULL,
	[Points] [int] NOT NULL,
	[ProfilePicture] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

