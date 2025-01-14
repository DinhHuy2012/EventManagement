USE [master]
GO
/****** Object:  Database [Asm3_EventManagement]    Script Date: 11/29/2024 1:49:11 PM ******/
CREATE DATABASE [Asm3_EventManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Asm3_EventManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Asm3_EventManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Asm3_EventManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Asm3_EventManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Asm3_EventManagement] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Asm3_EventManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Asm3_EventManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Asm3_EventManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Asm3_EventManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Asm3_EventManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Asm3_EventManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Asm3_EventManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Asm3_EventManagement] SET  MULTI_USER 
GO
ALTER DATABASE [Asm3_EventManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Asm3_EventManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Asm3_EventManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Asm3_EventManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Asm3_EventManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Asm3_EventManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Asm3_EventManagement] SET QUERY_STORE = OFF
GO
USE [Asm3_EventManagement]
GO
/****** Object:  Table [dbo].[Attendees]    Script Date: 11/29/2024 1:49:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendees](
	[AttendeeID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[EventID] [int] NULL,
	[Email] [nvarchar](255) NOT NULL,
	[RegistrationTime] [datetime] NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Attendee__184401285E73425F] PRIMARY KEY CLUSTERED 
(
	[AttendeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventCategories]    Script Date: 11/29/2024 1:49:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 11/29/2024 1:49:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Location] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[CategoryID] [int] NULL,
	[OrganizerID] [int] NULL,
	[Status] [int] NULL,
	[Image] [text] NULL,
	[Capacity] [int] NULL,
 CONSTRAINT [PK__Events__7944C8700FBA2950] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/29/2024 1:49:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Fullname] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[Role] [nvarchar](50) NOT NULL,
	[Avatar] [text] NULL,
	[Status] [bit] NULL,
	[JoinDate] [datetime] NULL,
 CONSTRAINT [PK__Users__1788CCAC7FCEBB0C] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Attendees] ON 

INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (2, 1, N'Huy Đình ', 2, N'jane@example.com', CAST(N'2024-08-15T10:07:59.350' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (3, 1, N'abc', 2, N'john@example.com', CAST(N'2024-08-14T10:07:59.350' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (4, 3, N'abc', 3, N'mike@example.com', CAST(N'2024-08-13T10:07:59.350' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (13, NULL, N'anhA', 14, N'anhA@gmail.com', CAST(N'2024-08-13T00:00:00.000' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (14, NULL, N'anhB', 14, N'anhB@gmail.com', CAST(N'2024-08-13T00:00:00.000' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (15, NULL, N'anhC', 14, N'anhC@gmail.com', CAST(N'2024-08-13T00:00:00.000' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (16, NULL, N'abc', 2, N'132@gmail.com', CAST(N'2024-08-20T11:19:30.590' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (17, NULL, N'Huy Đình ', 2, N'huydinh@gmail.com', CAST(N'2024-08-20T11:22:19.617' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (18, NULL, N'Huy Đình ', 14, N'huydinh@gmail.com', CAST(N'2024-08-20T11:22:55.030' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (19, NULL, N'123', 14, N'123@gmail.com', CAST(N'2024-08-20T11:23:34.400' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (20, NULL, N'a', 14, N'123@gmail.com', CAST(N'2024-08-20T11:23:42.697' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (21, NULL, N'132', 14, N'3232@gmail.com', CAST(N'2024-08-20T11:23:50.807' AS DateTime), N'Live')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (22, NULL, N'sdsd', 14, N'sds@Gmail.con', CAST(N'2024-08-20T11:23:56.427' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (23, NULL, N'132', 21, N'absdca@gmail.com', CAST(N'2024-08-20T11:41:38.137' AS DateTime), N'Reject')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (24, NULL, N'abac', 21, N'ddd@gmail.com', CAST(N'2024-08-21T09:44:53.013' AS DateTime), N'Pending')
INSERT [dbo].[Attendees] ([AttendeeID], [UserID], [Name], [EventID], [Email], [RegistrationTime], [Status]) VALUES (25, NULL, N'xuananhngu', 19, N'xuananhngu@gmail.com', CAST(N'2024-09-27T09:22:38.350' AS DateTime), N'Pending')
SET IDENTITY_INSERT [dbo].[Attendees] OFF
GO
SET IDENTITY_INSERT [dbo].[EventCategories] ON 

INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (1, N'Hội thảo', N'Hội thảo: Các buổi họp thảo luận chuyên môn về một chủ đề cụ thể.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (2, N'Đào tạo', N'Đào tạo: Sự kiện nhằm nâng cao kỹ năng và kiến thức.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (3, N'Giải trí', N'Giải trí: Các hoạt động vui chơi, giải trí như xem phim, kịch.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (4, N'Thể thao', N'Thể thao: Sự kiện thể thao, thi đấu hoặc tập luyện.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (5, N'Văn hóa', N'Văn hóa: Sự kiện liên quan đến văn hóa, lễ hội truyền thống.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (6, N'Nghệ thuật', N'Nghệ thuật: Triển lãm, biểu diễn liên quan đến mỹ thuật, âm nhạc, kịch nghệ.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (7, N'Khoa học', N'Khoa học: Sự kiện nghiên cứu, trao đổi học thuật, hội nghị khoa học.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (8, N'Công nghệ', N'Công nghệ: Hội nghị, triển lãm công nghệ và các sản phẩm kỹ thuật.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (9, N'Từ thiện', N'Từ thiện: Sự kiện gây quỹ, quyên góp cho các hoạt động xã hội.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (10, N'Âm nhạc', N'Âm nhạc: Các buổi hòa nhạc, biểu diễn âm nhạc trực tiếp.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (11, N'Doanh nghiệp', N'Doanh nghiệp: Hội nghị, hội thảo về kinh doanh, thương mại.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (12, N'Giáo dục', N'Giáo dục: Sự kiện học thuật, hội nghị giáo dục, hội thảo trường học.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (13, N'Y tế', N'Y tế: Hội nghị, hội thảo, sự kiện liên quan đến sức khỏe.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (14, N'Du lịch', N'Du lịch: Sự kiện quảng bá du lịch, hội chợ, triển lãm.
')
INSERT [dbo].[EventCategories] ([CategoryID], [CategoryName], [Description]) VALUES (15, N'Truyền thông
', N'Truyền thông: Sự kiện liên quan đến báo chí, truyền hình, mạng xã hội.
')
SET IDENTITY_INSERT [dbo].[EventCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Events] ON 

INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (2, N'AI Workshop', CAST(N'2024-10-20T10:00:00.000' AS DateTime), CAST(N'2024-10-30T15:00:00.000' AS DateTime), N'Community Center, City B', N'A workshop on artificial intelligence.', 2, 2, 3, N'04.jpg', 5)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (3, N'Cybersecurity Webinar', CAST(N'2023-10-25T14:30:00.000' AS DateTime), CAST(N'2023-10-26T16:00:00.000' AS DateTime), N'Hà Nội', N'An online seminar on cybersecurity best practices.', 3, 2, 1, N'02.jpg', 5)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (14, N'Em ơi Hà Nội Phố', CAST(N'2024-08-20T00:00:00.000' AS DateTime), CAST(N'2024-08-25T00:00:00.000' AS DateTime), N'Hà nội', N'Oke', 2, 1, 1, N'05.jpg', 7)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (18, N'Quốc kỳ Việt Nam Dân chủ Cộng hòa 2/9/2024', CAST(N'2024-09-01T10:19:00.000' AS DateTime), CAST(N'2024-09-03T10:19:00.000' AS DateTime), N'Ba Đình,Hà Nội', N'Sau Hiệp định Giơnevơ về Đông Dương, Mỹ và chính quyền Ngô Đình Diệm quay sang chống phá Hiệp định, phá hoại tổng tuyển cử để thống nhất nước nhà. Chúng đặt miền Nam trong tình trạng chiến tranh, thẳng tay tiến hành các chiến dịch đàn áp, khủng bố những người kháng chiến, người dân yêu nước và các lực lượng đối lập. Âm mưu xây dựng chế độ thống trị bằng bạo lực và máy chém, hòng chia cắt lâu dài đất nước Việt Nam và biến miền Nam Việt Nam thành thuộc địa kiểu mới của Mỹ. Trước tình đó, tại Đại hội đại biểu toàn quốc lần thứ III của Đảng (tháng 9/1960), Đảng ta đã chủ trương thành lập Trung ương Cục ở miền Nam để giúp Trung ương Đảng, Bộ Chính trị chỉ đạo phong trào cách mạng Miền Nam. Nhằm tập hợp rộng rãi lực lượng cách mạng và phát huy cao độ sức mạnh đại đoàn kết dân tộc, nêu cao chủ nghĩa yêu nước, Đại hội cũng chủ trương phải xây dựng tổ chức Mặt trận Dân tộc thống nhất ở miền Nam.', 3, 7, 3, N'08.jpg', 100)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (19, N'Quốc Khánh 2/9 Tại Quảng Trường Ba Đình Hà Nội.', CAST(N'2024-08-31T10:49:00.000' AS DateTime), CAST(N'2024-09-03T10:49:00.000' AS DateTime), N'Ba Đình,Hà Nội', N'Giữ gìn niềm tự tôn và bản sắc dân tộc Việt Nam.
', 2, 7, 1, N'hnoi-16936207347002000710774.png', 100)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (20, N'Quốc kỳ Việt Nam Dân chủ Cộng hòa 2/9/2024', CAST(N'2024-08-20T15:02:00.000' AS DateTime), CAST(N'2024-08-21T15:02:00.000' AS DateTime), N'Ba Đình,Hà Nội', N'hot', 3, 7, 1, N'07.jpg', 11)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (21, N'Quốc Khánh 2/9 Tại Quảng Trường Ba Đình Hà Nội.', CAST(N'2024-09-01T15:04:00.000' AS DateTime), CAST(N'2024-09-02T15:04:00.000' AS DateTime), N'Ba Đình,Hà Nội', N'hot', 1, 1, 1, N'hnoi-16936207347002000710774.png', 11)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (22, N'Quốc kỳ Việt Nam Dân chủ Cộng hòa 2/9/2024', CAST(N'2024-08-20T11:38:00.000' AS DateTime), CAST(N'2024-08-22T11:38:00.000' AS DateTime), N'Ba Đình,Hà Nội', N'abc', 1, 1, 3, N'10.jpg', 10)
INSERT [dbo].[Events] ([EventID], [Title], [StartTime], [EndTime], [Location], [Description], [CategoryID], [OrganizerID], [Status], [Image], [Capacity]) VALUES (24, N'Quốc kỳ Việt Nam Dân chủ Cộng hòa 2/9/2024', CAST(N'2024-09-25T09:34:00.000' AS DateTime), CAST(N'2024-09-29T09:34:00.000' AS DateTime), N'123', N'1231', 11, 7, 2, N'doc-dao-noi-dung-va-lich-chieu-1.png', 123)
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (1, N'admin', N'123', N'Huy Đình', N'admin@gmail.com', N'123456789', N'Admin', N'admin.png', 1, CAST(N'2023-10-25T14:00:00.000' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (2, N'jane_smith', N'123', N'Anh Huy', N'jane@example.com', N'0328758801', N'Người Đăng Kí', N'08.jpg', 1, CAST(N'2023-10-25T14:00:00.000' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (3, N'mike_jones', N'mypassword', N'Mike Jones', N'mike@example.com', N'555123456', N'Người Đăng Kí', N'07.jpg', 1, CAST(N'2023-10-25T14:00:00.000' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (7, N'huypd', N'1234', N'huypd', N'g@gmail.com', N'0328758801', N'Người Đăng Kí', N'07.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (11, N'huypds', N'123', N'Huy Dinh', N'g2@gmail.com', N'012312323', N'Người Đăng Kí', N'10.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (15, N'huypda', N'123', N'Huy Dinh', N'g3@gmail.com', N'012312323', N'Người Đăng Kí', N'01.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (16, N'huypdss', N'123', N'Huy Dinh', N'g4@gmail.com', N'012312323', N'Người Đăng Kí', N'06.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (18, N'huypdssss', N'123', N'Huy Dinh', N'g5@gmail.com', N'012312323', N'Người Đăng Kí', N'04.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (19, N'huypdsâss', N'123', N'Huy Dinh', N'g6@gmail.com', N'012312323', N'Người Đăng Kí', N'03.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (21, N'huypdsâsssss', N'123', N'Huy Dinh', N'g7@gmail.com', N'012312323', N'Người Đăng Kí', N'03.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (22, N'huypdsâssssss', N'123', N'Huy Dinh', N'g8@gmail.com', N'012312323', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (23, N'abs', N'123', N'Huy Dinh', N'g10@gmail.com', N'012312323', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (24, N'aaaa', N'123', N'Huy Dinh', N'g10=1@gmail.com', N'012312323', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (29, N'abc1', N'123', N'abccc', N'abc1@gmail.com', N'123', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (30, N'abc2', N'123', N'abc', N'abc2@gmail.com', N'123', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (31, N'abc3', N'123', N'abccc', N'abc23@gmail.com', N'123', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (38, N'john_doe222', N'1234', N'john_doe', N'abc222@gmail.com', N'1234', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:19:58.530' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (40, N'john_doe111', N'12345', N'john_doe', N'abc2322@gmail.com', N'123', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:20:48.807' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (48, N'khanhmom', N'1234', N'NoName', N'khanhmom@gmail.com', N'NoName', N'Admin', N'avatar-trang.jpg', 1, CAST(N'2023-10-25T14:00:00.000' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (49, N'khanhmom123', N'1234', N'NoName', N'khanhmoms@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:41:31.350' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (50, N'Huyentrang', N'1234', N'NoName', N'huyentrang@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:43:27.373' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (52, N'sdsds', N'1234', N'NoName', N'john_doe222@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:49:10.507' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (53, N'bbbb', N'bbbb', N'NoName', N'bbbb@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-14T23:51:04.903' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (61, N'vanhoang', N'1234', N'NoName', N'22222@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-15T00:00:32.280' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (62, N'xuananh', N'1234', N'NoName', N'xuananh@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-15T00:01:22.410' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (64, N'john_doess', N'1234', N'NoName', N'john_doesss@gmail.com', N'trống', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-15T00:04:28.783' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (65, N'xuananhngoungu', N'1234', N'NoName', N'xuannanh@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-15T08:26:13.543' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (70, N'anhhuydd', N'1234', N'asss', N'anhhuydd@gmail.com', N'trống', N'Người Đăng Kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-17T17:30:28.303' AS DateTime))
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Fullname], [Email], [Phone], [Role], [Avatar], [Status], [JoinDate]) VALUES (71, N'admin1', N'123', N'NoName', N'john_doe@gmail.com', N'trống', N'Người đăng kí', N'avatar-trang.jpg', 1, CAST(N'2024-08-20T11:36:45.520' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4B91AEAA9]    Script Date: 11/29/2024 1:49:11 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ__Users__536C85E4B91AEAA9] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D1053453A222CA]    Script Date: 11/29/2024 1:49:11 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ__Users__A9D1053453A222CA] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attendees] ADD  CONSTRAINT [DF__Attendees__Regis__403A8C7D]  DEFAULT (getdate()) FOR [RegistrationTime]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Fullname]  DEFAULT (N'NoName') FOR [Fullname]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Phone]  DEFAULT (N'trống') FOR [Phone]
GO
ALTER TABLE [dbo].[Attendees]  WITH CHECK ADD  CONSTRAINT [FK__Attendees__Event__4222D4EF] FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[Attendees] CHECK CONSTRAINT [FK__Attendees__Event__4222D4EF]
GO
ALTER TABLE [dbo].[Attendees]  WITH CHECK ADD  CONSTRAINT [FK__Attendees__UserI__412EB0B6] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Attendees] CHECK CONSTRAINT [FK__Attendees__UserI__412EB0B6]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK__Events__Category__3C69FB99] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[EventCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK__Events__Category__3C69FB99]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK__Events__Organize__3D5E1FD2] FOREIGN KEY([OrganizerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK__Events__Organize__3D5E1FD2]
GO
USE [master]
GO
ALTER DATABASE [Asm3_EventManagement] SET  READ_WRITE 
GO
