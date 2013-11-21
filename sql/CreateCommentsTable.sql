USE [DealStealUnreal]
GO

/****** Object:  Table [dbo].[Comments]    Script Date: 11/21/2013 07:10:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Comments](
	[Comment] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Username] [nvarchar](10) NOT NULL,
	[DealId] [int] NOT NULL
) ON [PRIMARY]

GO

