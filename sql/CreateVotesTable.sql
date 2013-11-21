USE [DealStealUnreal]
GO

/****** Object:  Table [dbo].[Votes]    Script Date: 11/21/2013 07:09:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Votes](
	[Vote] [int] NOT NULL,
	[Username] [nchar](10) NOT NULL,
	[Date] [datetime] NOT NULL,
	[DealId] [int] NOT NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Username] ASC,
	[DealId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD FOREIGN KEY([DealId])
REFERENCES [dbo].[Deals] ([DealId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD FOREIGN KEY([Username])
REFERENCES [dbo].[Users] ([Username])
GO