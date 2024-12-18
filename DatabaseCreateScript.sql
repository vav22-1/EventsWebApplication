USE [master]
GO
/****** Object:  Database [EventWebApplication]    Script Date: 19.12.2024 14:46:45 ******/
CREATE DATABASE [EventWebApplication]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EventWebApplication', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EventWebApplication.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EventWebApplication_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\EventWebApplication_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EventWebApplication] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EventWebApplication].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EventWebApplication] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EventWebApplication] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EventWebApplication] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EventWebApplication] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EventWebApplication] SET ARITHABORT OFF 
GO
ALTER DATABASE [EventWebApplication] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EventWebApplication] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EventWebApplication] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EventWebApplication] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EventWebApplication] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EventWebApplication] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EventWebApplication] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EventWebApplication] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EventWebApplication] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EventWebApplication] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EventWebApplication] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EventWebApplication] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EventWebApplication] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EventWebApplication] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EventWebApplication] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EventWebApplication] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EventWebApplication] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EventWebApplication] SET RECOVERY FULL 
GO
ALTER DATABASE [EventWebApplication] SET  MULTI_USER 
GO
ALTER DATABASE [EventWebApplication] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EventWebApplication] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EventWebApplication] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EventWebApplication] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EventWebApplication] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EventWebApplication] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EventWebApplication', N'ON'
GO
ALTER DATABASE [EventWebApplication] SET QUERY_STORE = ON
GO
ALTER DATABASE [EventWebApplication] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EventWebApplication]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19.12.2024 14:46:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventParticipants]    Script Date: 19.12.2024 14:46:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventParticipants](
	[EventId] [int] NOT NULL,
	[ParticipantId] [int] NOT NULL,
 CONSTRAINT [PK_EventParticipants] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[ParticipantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 19.12.2024 14:46:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DateAndTime] [datetime2](7) NOT NULL,
	[Location] [nvarchar](max) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[MaxParticipants] [int] NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 19.12.2024 14:46:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantId] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participants]    Script Date: 19.12.2024 14:46:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[DateOfRegistration] [datetime2](7) NULL,
	[Email] [nvarchar](max) NULL,
 CONSTRAINT [PK_Participants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19.12.2024 14:46:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [varbinary](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
	[RefreshToken] [nvarchar](max) NOT NULL,
	[RefreshTokenExpiry] [datetime2](7) NOT NULL,
	[ParticipantId] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_EventParticipants_ParticipantId]    Script Date: 19.12.2024 14:46:45 ******/
CREATE NONCLUSTERED INDEX [IX_EventParticipants_ParticipantId] ON [dbo].[EventParticipants]
(
	[ParticipantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_ParticipantId]    Script Date: 19.12.2024 14:46:45 ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_ParticipantId] ON [dbo].[Notifications]
(
	[ParticipantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_ParticipantId]    Script Date: 19.12.2024 14:46:45 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_ParticipantId] ON [dbo].[Users]
(
	[ParticipantId] ASC
)
WHERE ([ParticipantId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventParticipants]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipants_Events_EventId] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventParticipants] CHECK CONSTRAINT [FK_EventParticipants_Events_EventId]
GO
ALTER TABLE [dbo].[EventParticipants]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipants_Participants_ParticipantId] FOREIGN KEY([ParticipantId])
REFERENCES [dbo].[Participants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventParticipants] CHECK CONSTRAINT [FK_EventParticipants_Participants_ParticipantId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Participants_ParticipantId] FOREIGN KEY([ParticipantId])
REFERENCES [dbo].[Participants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Participants_ParticipantId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Participants_ParticipantId] FOREIGN KEY([ParticipantId])
REFERENCES [dbo].[Participants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Participants_ParticipantId]
GO
USE [master]
GO
ALTER DATABASE [EventWebApplication] SET  READ_WRITE 
GO
