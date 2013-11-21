USE [DealStealUnreal]
GO

/****** Object:  Table [dbo].[Deals]    Script Date: 11/21/2013 07:10:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deals](
	[DealId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](10) NOT NULL,
	[Title] [nvarchar](30) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Retailer] [nvarchar](30) NOT NULL,
	[Url] [nvarchar](1000) NOT NULL,
	[Price] [real] NOT NULL,
	[ImageUrl] [nvarchar](1000) NULL,
	[Date] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Deals] PRIMARY KEY CLUSTERED 
(
	[DealId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

