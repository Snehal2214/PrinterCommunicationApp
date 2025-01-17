USE [master]
GO
/****** Object:  Database [VideojetAPP]    Script Date: 29-11-2024 11:00:22 ******/
CREATE DATABASE [VideojetAPP]
GO
USE [VideojetAPP]
GO
/****** Object:  Table [dbo].[LoginDetails]    Script Date: 29-11-2024 11:00:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginDetails](
	[Id] [int] NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](15) NOT NULL,
	[Role] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ChangeUserPassword]    Script Date: 29-11-2024 11:00:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangeUserPassword]
    @Username NVARCHAR(50),
    @CurrentPassword NVARCHAR(50),
    @NewPassword NVARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username AND Password = @CurrentPassword)
    BEGIN
        UPDATE Users
        SET Password = @NewPassword
        WHERE Username = @Username;
        
        RETURN 1; -- Success
    END
    ELSE
    BEGIN
        RETURN 0; -- Failure
    END
END
GO
/****** Object:  StoredProcedure [dbo].[Checklogin]    Script Date: 29-11-2024 11:00:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Checklogin]
 @UserName VARCHAR(45),
 @Password VARCHAR(45)
AS
BEGIN
    -- Select the role for the given username and password
    SELECT Role
    FROM Login
    WHERE Username = @Username AND Password = @Password;
END;
GO
/****** Object:  StoredProcedure [dbo].[Checklogins]    Script Date: 29-11-2024 11:00:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Checklogins]
 @UserName VARCHAR(45),
 @Password VARCHAR(45)
AS
BEGIN
    -- Select the role for the given username and password
    SELECT Role
    FROM LoginDetails
    WHERE Username = @Username AND Password = @Password;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAllLoginDetails]    Script Date: 29-11-2024 11:00:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllLoginDetails]
AS
BEGIN
    SELECT * FROM LoginDetails;
END;
GO
USE [master]
GO
ALTER DATABASE [VideojetAPP] SET  READ_WRITE 
GO
