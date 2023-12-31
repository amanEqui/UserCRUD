USE [Users]
GO
/****** Object:  Table [dbo].[Skills]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skills](
	[SkillID] [int] IDENTITY(1,1) NOT NULL,
	[SkillName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[SkillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[UserPass] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[Status] [int] NULL,
	[Phone] [bigint] NULL,
	[Address] [varchar](200) NULL,
	[Email] [varchar](50) NULL,
	[DOB] [varchar](20) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSkills]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSkills](
	[UserID] [int] NOT NULL,
	[SkillID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[SkillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Skills] ON 

INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (1, N'Java')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (2, N'Python')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (3, N'DotNet')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (4, N'C#')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (5, N'C++')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (6, N'React')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (7, N'MS Excel')
INSERT [dbo].[Skills] ([SkillID], [SkillName]) VALUES (8, N'MS Word')
SET IDENTITY_INSERT [dbo].[Skills] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserID], [UserName], [UserPass], [Gender], [Status], [Phone], [Address], [Email], [DOB]) VALUES (46, N'bipin', N'123', N'Male', 1, 9876543210, N'bhandup', N'abc@gmail.com', N'2023-06-15')
INSERT [dbo].[User] ([UserID], [UserName], [UserPass], [Gender], [Status], [Phone], [Address], [Email], [DOB]) VALUES (47, N'arun', N'123', N'Male', 1, 1236546542, N'kurla', N'def@gmail.com', N'2023-05-04')
SET IDENTITY_INSERT [dbo].[User] OFF
GO
INSERT [dbo].[UserSkills] ([UserID], [SkillID]) VALUES (46, 1)
INSERT [dbo].[UserSkills] ([UserID], [SkillID]) VALUES (47, 1)
INSERT [dbo].[UserSkills] ([UserID], [SkillID]) VALUES (47, 2)
INSERT [dbo].[UserSkills] ([UserID], [SkillID]) VALUES (47, 4)
INSERT [dbo].[UserSkills] ([UserID], [SkillID]) VALUES (47, 5)
INSERT [dbo].[UserSkills] ([UserID], [SkillID]) VALUES (47, 6)
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UserSkills]  WITH CHECK ADD FOREIGN KEY([SkillID])
REFERENCES [dbo].[Skills] ([SkillID])
GO
ALTER TABLE [dbo].[UserSkills]  WITH CHECK ADD  CONSTRAINT [FK__UserSkill__UserI__3F466844] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSkills] CHECK CONSTRAINT [FK__UserSkill__UserI__3F466844]
GO
/****** Object:  StoredProcedure [dbo].[SkillAdd]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[SkillAdd]
@SkillID int,
@SkillName varchar(50)
As
IF @SkillID = 0
Insert into [Skills](SkillName) values (@SkillName)
Else
Update [Skills]
set
SkillName = @SkillName
where SkillID = @SkillID
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[UpdateUser]
@UserID int,
@UserName varchar(50),
@UserPass varchar(50),
@DOB varchar(20),
@Gender varchar(50),
@Phone bigint,
@Address varchar(200),
@Email varchar(50)
As
Update [User]
set
UserName = @UserName,
UserPass = @UserPass,
DOB = @DOB,
Gender = @Gender,
Phone = @Phone,
Address = @Address,
Email = @Email

where UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[UserAddorEdit]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[UserAddorEdit]
@UserID int,
@UserName varchar(50),
@UserPass varchar(50),
@DOB varchar(20),
@Gender varchar(50),
@Phone bigint,
@Address varchar(200),
@Email varchar(50)

AS

IF @UserID =0
Insert into [User](UserName,UserPass,DOB,Gender,Phone,Address,Email)
values (@UserName,@UserPass,@DOB,@Gender,@Phone,@Address,@Email)
Else
Update [User]
set
UserName = @UserName,
UserPass = @UserPass,
DOB = @DOB,
Gender = @Gender,
Phone = @Phone,
Address = @Address,
Email = @Email

where UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[UserDeleteByID]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[UserDeleteByID]
@UserID int
As
Delete from [User] where UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[UserHide]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[UserHide]
@UserID int
As
Update [User] Set Status = 0 where UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[UserRestore]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[UserRestore]
As
Select * from [User] where Status = 0
GO
/****** Object:  StoredProcedure [dbo].[UserRestoreByID]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[UserRestoreByID]
@UserID int
As
Update [User] Set Status = 1 where UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[UserViewAll]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[UserViewAll]
As
Select * from [User] where Status = 1
GO
/****** Object:  StoredProcedure [dbo].[UserViewByID]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[UserViewByID]
@UserID int
As
Select * from [User] where UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[ViewAll]    Script Date: 20-06-2023 19:04:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[ViewAll] 
As
SELECT * from [User] u
                JOIN UserSkills us ON u.UserID = us.UserID
                JOIN Skills s ON us.SkillID = s.SkillID where u.Status = 1;
GO
