Use [master]
GO

/****** Object:  Database [EmployeePerformance] Script Date: 09/05/2021 3:19 PM ******/

IF NOT EXISTS (Select 1 From  sys.databases WHERE name = N'EmployeePerformance')
CREATE DATABASE [EmployeePerformance]
GO

USE [EmployeePerformance]
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Position' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[Position](
		[Id] [int] IDENTITY(1, 1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
    CREATE TABLE [dbo].[User] (
        [Id] [int] IDENTITY(1, 1) NOT NULL,
        [Email] [nvarchar](75) NOT NULL,
        [FirstName] [nvarchar](50) NOT NULL,
        [LastName] [nvarchar](50) NOT NULL,
        [PositionId] [int] NOT NULL,
        [Sex] [bit] NULL,
        [DoB] [datetime] NOT NULL,
        [PhoneNo] [nvarchar](10) NOT NULL,
        [RoleId] [int] NOT NULL,
        [PasswordHash] [nvarchar](500) NOT NULL,
        [PasswordSalt] [nvarchar](50) NOT NULL,
        [PasswordResetCode] [nvarchar] (500) NULL,
        [Rowversion] [timestamp] NOT NULL,
        [CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProjectUser' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[ProjectUser](
		[Id] [uniqueidentifier] NOT NULL,
		[ProjectId] [int] NOT NULL,
        [UserId] [int] NOT NULL,
        [ProjectRoleId] [int] NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_ProjectUser] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Project' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[Project](
		[Id] [int] IDENTITY(1, 1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
        [Description] [nvarchar] (500) NULL,
        [Status] [int] NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CriteriaType' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[CriteriaType](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
        [Description] [nvarchar] (500) NULL,
        [OrderNo] [int] NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_CriteriaType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Criteria' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[Criteria](
		[Id] [uniqueidentifier] NOT NULL,
        [CriteriaTypeId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
        [Description] [nvarchar] (500) NULL,
        [OrderNo] [int] NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_Criteria] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CriteriaQuarterEvaluation' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[CriteriaQuarterEvaluation](
		[Id] [uniqueidentifier] NOT NULL,
		[QuarterEvaluationId] [uniqueidentifier] NOT NULL,
        [CriteriaId] [uniqueidentifier] NOT NULL,
        [Point] [bigint] NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
	CONSTRAINT [PK_CriteriaQuarterEvaluation] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CriteriaTypeQuarterEvaluation' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[CriteriaTypeQuarterEvaluation](
		[Id] [uniqueidentifier] NOT NULL,
		[QuarterEvaluationId] [uniqueidentifier] NOT NULL,
        [CriteriaTypeId] [uniqueidentifier] NOT NULL,
        [PointAverage] [float] NOT NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
	CONSTRAINT [PK_CriteriaTypeQuarterEvaluation] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QuarterEvaluation' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[QuarterEvaluation](
		[Id] [uniqueidentifier] NOT NULL,
		[Year] [int] NOT NULL,
        [Quarter] [int] NOT NULL,
        [UserId] [int] NOT NULL,
        [PositionId] [int] NOT NULL,
        [ProjectId] [int] NOT NULL,
        ProjectLeaderId [int] NOT NULL,
        [PointAverage] [float] NOT NULL,
        [NoteByLeader] [nvarchar] (500) NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_QuarterEvaluation] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserQuarterEvaluation' AND TABLE_TYPE = 'BASE TABLE')
BEGIN
	CREATE TABLE [dbo].[UserQuarterEvaluation](
		[Id] [uniqueidentifier] NOT NULL,
		[QuarterEvaluationId] [uniqueidentifier] NOT NULL UNIQUE FOREIGN KEY REFERENCES [dbo].[QuarterEvaluation](Id),
        [NoteGoodThing] [nvarchar] (500) NULL,
        [NoteBadThing] [nvarchar] (500) NULL,
        [NoteOther] [nvarchar] (500) NULL,
        [Rowversion] [timestamp] NOT NULL,
		[CreatedBy] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [int] NULL,
		[ModifiedDate] [datetime] NULL,
		[DeletedBy] [int] NULL,
		[DeletedDate] [datetime] NULL,
	CONSTRAINT [PK_UserQuarterEvaluation] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

-- TABLE_CONSTRAINTS

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'User' AND CONSTRAINT_NAME = 'FK_User_Position')
BEGIN
    ALTER TABLE [dbo].[User] WITH CHECK ADD CONSTRAINT [FK_User_Position] FOREIGN KEY ([PositionId])
    REFERENCES [dbo].[Position] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'User' AND CONSTRAINT_NAME = 'FK_User_Position')
BEGIN
    ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Position]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'ProjectUser' AND CONSTRAINT_NAME = 'FK_ProjectUser_User')
BEGIN
    ALTER TABLE [dbo].[ProjectUser] WITH CHECK ADD CONSTRAINT [FK_ProjectUser_User] FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'ProjectUser' AND CONSTRAINT_NAME = 'FK_ProjectUser_User')
BEGIN
    ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_User]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'ProjectUser' AND CONSTRAINT_NAME = 'FK_ProjectUser_Project')
BEGIN
    ALTER TABLE [dbo].[ProjectUser] WITH CHECK ADD CONSTRAINT [FK_ProjectUser_Project] FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[Project] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'ProjectUser' AND CONSTRAINT_NAME = 'FK_ProjectUser_Project')
BEGIN
    ALTER TABLE [dbo].[ProjectUser] CHECK CONSTRAINT [FK_ProjectUser_Project]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'Criteria' AND CONSTRAINT_NAME = 'FK_Criteria_CriteriaType')
BEGIN
    ALTER TABLE [dbo].[Criteria] WITH CHECK ADD CONSTRAINT [FK_Criteria_CriteriaType] FOREIGN KEY ([CriteriaTypeId])
    REFERENCES [dbo].[CriteriaType] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'Criteria' AND CONSTRAINT_NAME = 'FK_Criteria_CriteriaType')
BEGIN
    ALTER TABLE [dbo].[Criteria] CHECK CONSTRAINT [FK_Criteria_CriteriaType]
END


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaQuarterEvaluation_Criteria')
BEGIN
    ALTER TABLE [dbo].[CriteriaQuarterEvaluation] WITH CHECK ADD CONSTRAINT [FK_CriteriaQuarterEvaluation_Criteria] FOREIGN KEY ([CriteriaId])
    REFERENCES [dbo].[Criteria] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaQuarterEvaluation_Criteria')
BEGIN
    ALTER TABLE [dbo].[CriteriaQuarterEvaluation] CHECK CONSTRAINT [FK_CriteriaQuarterEvaluation_Criteria]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaQuarterEvaluation_QuarterEvaluation')
BEGIN
    ALTER TABLE [dbo].[CriteriaQuarterEvaluation] WITH CHECK ADD CONSTRAINT [FK_CriteriaQuarterEvaluation_QuarterEvaluation] FOREIGN KEY ([QuarterEvaluationId])
    REFERENCES [dbo].[QuarterEvaluation] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaQuarterEvaluation_QuarterEvaluation')
BEGIN
    ALTER TABLE [dbo].[CriteriaQuarterEvaluation] CHECK CONSTRAINT [FK_CriteriaQuarterEvaluation_QuarterEvaluation]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaTypeQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaTypeQuarterEvaluation_CriteriaType')
BEGIN
    ALTER TABLE [dbo].[CriteriaTypeQuarterEvaluation] WITH CHECK ADD CONSTRAINT [FK_CriteriaTypeQuarterEvaluation_CriteriaType] FOREIGN KEY ([CriteriaTypeId])
    REFERENCES [dbo].[CriteriaType] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaTypeQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaTypeQuarterEvaluation_CriteriaType')
BEGIN
    ALTER TABLE [dbo].[CriteriaTypeQuarterEvaluation] CHECK CONSTRAINT [FK_CriteriaTypeQuarterEvaluation_CriteriaType]
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaTypeQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaTypeQuarterEvaluation_QuarterEvaluation')
BEGIN
    ALTER TABLE [dbo].[CriteriaTypeQuarterEvaluation] WITH CHECK ADD CONSTRAINT [FK_CriteriaTypeQuarterEvaluation_QuarterEvaluation] FOREIGN KEY ([QuarterEvaluationId])
    REFERENCES [dbo].[QuarterEvaluation] ([Id])
END
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS Where TABLE_NAME = 'CriteriaTypeQuarterEvaluation' AND CONSTRAINT_NAME = 'FK_CriteriaTypeQuarterEvaluation_QuarterEvaluation')
BEGIN
    ALTER TABLE [dbo].[CriteriaTypeQuarterEvaluation] CHECK CONSTRAINT [FK_CriteriaTypeQuarterEvaluation_QuarterEvaluation]
END


-- INIT POSITION

IF NOT EXISTS (SELECT 1 FROM Position Where Name = 'Admin')
	INSERT INTO Position (Name, CreatedBy, CreatedDate) VALUES ('Admin', 1, GETDATE())
GO

IF NOT EXISTS (SELECT 1 FROM Position Where Name = 'Developer')
	INSERT INTO Position (Name, CreatedBy, CreatedDate) VALUES ('Developer', 1, GETDATE())
GO

IF NOT EXISTS (SELECT 1 FROM Position Where Name = 'Tester')
	INSERT INTO Position (Name, CreatedBy, CreatedDate) VALUES ('Tester', 1, GETDATE())
GO

IF NOT EXISTS (SELECT 1 FROM Position Where Name = 'QA')
	INSERT INTO Position (Name, CreatedBy, CreatedDate) VALUES ('QA', 1, GETDATE())
GO

IF NOT EXISTS (SELECT 1 FROM Position Where Name = 'Language Assistant')
	INSERT INTO Position (Name, CreatedBy, CreatedDate) VALUES ('Language Assistant', 1, GETDATE())
GO


IF NOT EXISTS (SELECT 1 FROM Position Where Name = 'Intern')
	INSERT INTO Position (Name, CreatedBy, CreatedDate) VALUES ('Intern', 1, GETDATE())
GO

-- DEFAULT ADMIN
IF NOT EXISTS (SELECT * FROM [User] WHERE [Email] = 'admin@kloon.com')
BEGIN
	DECLARE @positionId INT
	SELECT @positionId = Id FROM Position WHERE Name = 'Admin'

	INSERT INTO [User] (Email, FirstName, LastName, DoB, PhoneNo, RoleId, PositionId, Sex, PasswordHash, PasswordSalt, CreatedDate, CreatedBy)
	VALUES
	(
		'admin@kloon.vn',
		'Admin',
		'Admin',
		'1980-1-1',
		'123456789',
		1,
		@positionId,
		1,
		'7CD4A46E7B0F31EFBFBD1F2B5E7969EFBFBD4EEFBFBDEFBFBDEFBFBD6A6E15EFBFBDEFBFBD3DEFBFBD30D69E44EFBFBD4425',
		'84b32f39-a6d5-4d5a-908c-538fea22b3d9',
		GETDATE(),
		1
	)
END
GO
