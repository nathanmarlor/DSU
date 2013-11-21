USE [DealStealUnreal]
GO

/****** Object:  Table [dbo].[Sessions]    Script Date: 11/21/2013 07:10:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sessions](
	[SessionId] [nvarchar](50) NOT NULL,
	[LastUpdatedTime] [datetime] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[RememberMe] [bit] NOT NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

