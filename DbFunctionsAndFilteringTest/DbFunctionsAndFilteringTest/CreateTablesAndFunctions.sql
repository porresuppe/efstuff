SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fruit](
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Family] [nvarchar](200) NOT NULL,
	[Created] [datetime2](7) NOT NULL CONSTRAINT [DF_Fruit_Created]  DEFAULT (getdate()),
	[Deleted] [bit] NOT NULL default 0,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[Fruit] ([Name], [Description], [Family]) VALUES (N'Ananas', NULL, N'Bromeliaceae')
GO
INSERT [dbo].[Fruit] ([Name], [Description], [Family]) VALUES (N'Banan', NULL, N'Musaceae')
GO
INSERT [dbo].[Fruit] ([Name], [Description], [Family]) VALUES (N'Plantain', NULL, N'Musaceae')
GO
INSERT [dbo].[Fruit] ([Name], [Description], [Family], [Deleted]) VALUES (N'Rådhus-Vin', NULL, N'Vitaceae', 1)
GO


CREATE FUNCTION [dbo].[FamilyCount] (@family nvarchar(200) )
returns INT
as
begin
	declare @count int;
	
	SELECT @count=count(*) from Fruit where Fruit.Family = @family
		
	IF @@ERROR <>0
		RETURN -1
	
	return @count
END

GO


