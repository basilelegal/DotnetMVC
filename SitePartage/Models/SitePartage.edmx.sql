
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/21/2018 17:07:57
-- Generated from EDMX file: C:\Users\Shalom\source\repos\SitePartage\SitePartage\Models\SitePartage.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SitePartageEntities];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Comment_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK_Comment_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_Comment_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comment] DROP CONSTRAINT [FK_Comment_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Leasing_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Leasing] DROP CONSTRAINT [FK_Leasing_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_Leasing_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Leasing] DROP CONSTRAINT [FK_Leasing_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_Product_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_Product_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Comment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comment];
GO
IF OBJECT_ID(N'[dbo].[Leasing]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Leasing];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(15)  NOT NULL,
    [Picture] nvarchar(max)  NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [CommentID] int IDENTITY(1,1) NOT NULL,
    [ProductID] int  NULL,
    [UserID] int  NULL,
    [Title] nvarchar(40)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Date] datetime  NULL,
    [Status] nvarchar(20)  NULL
);
GO

-- Creating table 'Leasings'
CREATE TABLE [dbo].[Leasings] (
    [LeasingID] int IDENTITY(1,1) NOT NULL,
    [ProductID] int  NULL,
    [UserID] int  NULL,
    [Date] datetime  NULL,
    [Duration] int  NULL,
    [TotalCost] int  NULL,
    [Status] nvarchar(20)  NULL,
    [HasComment] bit  NULL,
    [Message] varchar(max)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [ProductID] int IDENTITY(1,1) NOT NULL,
    [CategoryID] int  NULL,
    [UserID] int  NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Cost] int  NULL,
    [Picture] nvarchar(max)  NULL,
    [Type] nvarchar(20)  NOT NULL,
    [Weight] float  NULL,
    [Status] nvarchar(20)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [LastName] nvarchar(30)  NULL,
    [FirstName] nvarchar(30)  NULL,
    [Civility] nvarchar(30)  NULL,
    [NickName] nvarchar(30)  NULL,
    [Email] nvarchar(30)  NULL,
    [Password] nvarchar(30)  NULL,
    [NbPoint] int  NULL,
    [Address] nvarchar(60)  NULL,
    [PostalCode] nvarchar(10)  NULL,
    [City] nvarchar(15)  NULL,
    [Role] nvarchar(10)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CategoryID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryID] ASC);
GO

-- Creating primary key on [CommentID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([CommentID] ASC);
GO

-- Creating primary key on [LeasingID] in table 'Leasings'
ALTER TABLE [dbo].[Leasings]
ADD CONSTRAINT [PK_Leasings]
    PRIMARY KEY CLUSTERED ([LeasingID] ASC);
GO

-- Creating primary key on [ProductID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([ProductID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Product_Category]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([CategoryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_Category'
CREATE INDEX [IX_FK_Product_Category]
ON [dbo].[Products]
    ([CategoryID]);
GO

-- Creating foreign key on [ProductID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comment_Product]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[Products]
        ([ProductID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comment_Product'
CREATE INDEX [IX_FK_Comment_Product]
ON [dbo].[Comments]
    ([ProductID]);
GO

-- Creating foreign key on [UserID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comment_User]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comment_User'
CREATE INDEX [IX_FK_Comment_User]
ON [dbo].[Comments]
    ([UserID]);
GO

-- Creating foreign key on [ProductID] in table 'Leasings'
ALTER TABLE [dbo].[Leasings]
ADD CONSTRAINT [FK_Leasing_Product]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[Products]
        ([ProductID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Leasing_Product'
CREATE INDEX [IX_FK_Leasing_Product]
ON [dbo].[Leasings]
    ([ProductID]);
GO

-- Creating foreign key on [UserID] in table 'Leasings'
ALTER TABLE [dbo].[Leasings]
ADD CONSTRAINT [FK_Leasing_User]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Leasing_User'
CREATE INDEX [IX_FK_Leasing_User]
ON [dbo].[Leasings]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Product_User]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_User'
CREATE INDEX [IX_FK_Product_User]
ON [dbo].[Products]
    ([UserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------