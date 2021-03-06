SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Family](
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Fruit](
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Created] [datetime2](7) NOT NULL CONSTRAINT [DF_Fruit_Created]  DEFAULT (getdate()),
	[Deleted] [bit] NOT NULL default 0,
	[FamilyId] [int] NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Fruit]  WITH CHECK ADD  CONSTRAINT [FK_Fruit_Family] FOREIGN KEY([FamilyId])
REFERENCES [dbo].[Family] ([Id])
GO
ALTER TABLE [dbo].[Fruit] CHECK CONSTRAINT [FK_Fruit_Family]
GO

SET IDENTITY_INSERT [dbo].[Family] ON
GO

INSERT [dbo].[Family] ([Id], [Name]) VALUES (1, N'Bromeliaceae')
GO
INSERT [dbo].[Family] ([Id], [Name]) VALUES (2, N'Musaceae')
GO
INSERT [dbo].[Family] ([Id], [Name]) VALUES (3, N'Vitaceae')
GO

SET IDENTITY_INSERT [dbo].[Family] OFF
GO

SET IDENTITY_INSERT [dbo].[Fruit] ON
GO

INSERT [dbo].[Fruit] ([Id], [Name], [Description], [Created], [Deleted], [FamilyId]) VALUES (1, N'Ananas', NULL, CAST(N'2017-01-20 15:44:18.7630000' AS DateTime2), 0, 1)
GO
INSERT [dbo].[Fruit] ([Id], [Name], [Description], [Created], [Deleted], [FamilyId]) VALUES (2, N'Banan', NULL, CAST(N'2017-01-20 15:44:18.7700000' AS DateTime2), 0, 2)
GO
INSERT [dbo].[Fruit] ([Id], [Name], [Description], [Created], [Deleted], [FamilyId]) VALUES (3, N'Plantain', NULL, CAST(N'2017-01-20 15:44:18.7700000' AS DateTime2), 0, 2)
GO
INSERT [dbo].[Fruit] ([Id], [Name], [Description], [Created], [Deleted], [FamilyId]) VALUES (4, N'Rådhus-Vin', NULL, CAST(N'2017-01-20 15:44:18.7730000' AS DateTime2), 1, 3)
GO

SET IDENTITY_INSERT [dbo].[Fruit] OFF
GO

CREATE FUNCTION [dbo].[FruitCount] (@family nvarchar(200) )
returns INT
as
begin
	declare @count int;
	
	SELECT @count=count(*) from Family, Fruit where Family.Id = Fruit.FamilyId and Family.Name = @family
		
	IF @@ERROR <>0
		RETURN -1
	
	return @count
END
GO