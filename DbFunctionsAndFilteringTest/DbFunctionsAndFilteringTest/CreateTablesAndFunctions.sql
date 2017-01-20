SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Frugt](
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Navn] [nvarchar](100) NOT NULL,
	[Beskrivelse] [nvarchar](max) NULL,
	[Familie] [nvarchar](200) NOT NULL,
	[Oprettet] [datetime2](7) NOT NULL CONSTRAINT [DF_Frugt_Oprettet]  DEFAULT (getdate()),
	[Slettet] [bit] NOT NULL default 0,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[Frugt] ([Navn], [Beskrivelse], [Familie]) VALUES (N'Ananas', NULL, N'Bromeliaceae')
GO
INSERT [dbo].[Frugt] ([Navn], [Beskrivelse], [Familie]) VALUES (N'Banan', NULL, N'Musaceae')
GO
INSERT [dbo].[Frugt] ([Navn], [Beskrivelse], [Familie]) VALUES (N'Plantain', NULL, N'Musaceae')
GO

CREATE FUNCTION [dbo].[FamilieCount] (@familie nvarchar(200) )
returns INT
as
begin
	declare @count int;
	
	SELECT @count=count(*) from Frugt where Frugt.Familie = @familie
		
	IF @@ERROR <>0
		RETURN -1
	
	return @count
END

GO


