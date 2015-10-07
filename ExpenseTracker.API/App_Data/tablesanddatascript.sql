
/****** Object:  Table [dbo].[Expense]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expense](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[Date] [date] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[ExpenseGroupId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
	))

 

GO
/****** Object:  Table [dbo].[ExpenseGroup]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[ExpenseGroupStatusId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))

 

GO
/****** Object:  Table [dbo].[ExpenseGroupStatus]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseGroupStatus](
	[Id] [int] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))
 

GO
SET IDENTITY_INSERT [dbo].[Expense] ON 

GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (1, N'Train tickets', CAST(N'2014-05-03' AS Date), CAST(63 AS Decimal(18, 0)), 1)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (3, N'Dinner', CAST(N'2014-05-03' AS Date), CAST(50 AS Decimal(18, 0)), 1)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (4, N'Train tickets', CAST(N'2014-02-10' AS Date), CAST(40 AS Decimal(18, 0)), 2)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (5, N'Entrance tickets', CAST(N'2014-05-03' AS Date), CAST(200 AS Decimal(18, 0)), 1)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (11, N'Lunch', CAST(N'2014-05-04' AS Date), CAST(35 AS Decimal(18, 0)), 1)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (14, N'Plane tickets', CAST(N'2014-06-03' AS Date), CAST(650 AS Decimal(18, 0)), 4)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (16, N'Entrance tickets', CAST(N'2014-04-01' AS Date), CAST(1200 AS Decimal(18, 0)), 6)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (17, N'Plane tickets', CAST(N'2014-01-03' AS Date), CAST(300 AS Decimal(18, 0)), 7)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (18, N'Dinner', CAST(N'2014-06-06' AS Date), CAST(60 AS Decimal(18, 0)), 12)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (20, N'Lunch', CAST(N'2014-05-01' AS Date), CAST(50 AS Decimal(18, 0)), 13)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (21, N'Lunch', CAST(N'2014-08-08' AS Date), CAST(40 AS Decimal(18, 0)), 14)
GO
INSERT [dbo].[Expense] ([Id], [Description], [Date], [Amount], [ExpenseGroupId]) VALUES (22, N'Dinner', CAST(N'2014-10-10' AS Date), CAST(30 AS Decimal(18, 0)), 17)
GO
SET IDENTITY_INSERT [dbo].[Expense] OFF
GO
SET IDENTITY_INSERT [dbo].[ExpenseGroup] ON 

GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (1, N'https://expensetrackeridsrv3/embedded_1', N'Web Development Conference', N'Web development conference in NYC', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (2, N'https://expensetrackeridsrv3/embedded_2', N'Contoso Prospect Visit', N'Possible prospect visit and dinner', 2)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (4, N'https://expensetrackeridsrv3/embedded_1', N'Business Trip UK', N'Trip to potential clients in the UK', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (6, N'https://expensetrackeridsrv3/embedded_3', N'Microsoft BUILD Conference', N'BUILD conference in San Francisco', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (7, N'https://expensetrackeridsrv3/embedded_1', N'Techdays Finland', N'Techdays conference in Finland - speaker trip', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (12, N'https://expensetrackeridsrv3/embedded_1', N'MADN User Group Conference', N'User group conference, all things Windows Phone and Store apps', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (13, N'https://expensetrackeridsrv3/embedded_4', N'Techorama Conference', N'Techorama conference in Belgium', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (14, N'https://expensetrackeridsrv3/embedded_1', N'Team Building Offsite', N'Team event with the company on an offsite location', 1)
GO
INSERT [dbo].[ExpenseGroup] ([Id], [UserId], [Title], [Description], [ExpenseGroupStatusId]) VALUES (17, N'https://expensetrackeridsrv3/embedded_1', N'VISUG event', N'Visual Studio user group event', 1)
GO
SET IDENTITY_INSERT [dbo].[ExpenseGroup] OFF
GO
INSERT [dbo].[ExpenseGroupStatus] ([Id], [Description]) VALUES (1, N'Open')
GO
INSERT [dbo].[ExpenseGroupStatus] ([Id], [Description]) VALUES (2, N'Confirmed')
GO
INSERT [dbo].[ExpenseGroupStatus] ([Id], [Description]) VALUES (3, N'Processed')
GO
ALTER TABLE [dbo].[Expense]  WITH CHECK ADD  CONSTRAINT [FK_Expense_ExpenseGroup] FOREIGN KEY([ExpenseGroupId])
REFERENCES [dbo].[ExpenseGroup] ([Id])
GO
ALTER TABLE [dbo].[Expense] CHECK CONSTRAINT [FK_Expense_ExpenseGroup]
GO
ALTER TABLE [dbo].[ExpenseGroup]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseGroup_ExpenseGroupStatus] FOREIGN KEY([ExpenseGroupStatusId])
REFERENCES [dbo].[ExpenseGroupStatus] ([Id])
GO
ALTER TABLE [dbo].[ExpenseGroup] CHECK CONSTRAINT [FK_ExpenseGroup_ExpenseGroupStatus]
GO
