IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [EVENT] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(150) NOT NULL,
        [EventDate] datetime2 NOT NULL,
        [Venue] nvarchar(150) NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [ImageUrl] nvarchar(500) NULL,
        [Description] nvarchar(2000) NULL,
        CONSTRAINT [PK_EVENT] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [IDENTITY_ROLE] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_IDENTITY_ROLE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [USER] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [UserName] nvarchar(150) NULL,
        [NormalizedUserName] nvarchar(150) NULL,
        [Email] nvarchar(150) NULL,
        [NormalizedEmail] nvarchar(150) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_USER] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [SECTOR] (
        [Id] int NOT NULL IDENTITY,
        [EventId] int NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Capacity] int NOT NULL,
        CONSTRAINT [PK_SECTOR] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SECTOR_EVENT_EventId] FOREIGN KEY ([EventId]) REFERENCES [EVENT] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [IDENTITY_ROLE_CLAIM] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_IDENTITY_ROLE_CLAIM] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IDENTITY_ROLE_CLAIM_IDENTITY_ROLE_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [IDENTITY_ROLE] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [AUDIT_LOG] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] int NULL,
        [Action] nvarchar(100) NOT NULL,
        [EntityType] nvarchar(100) NOT NULL,
        [EntityId] nvarchar(100) NOT NULL,
        [Details] nvarchar(1000) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_AUDIT_LOG] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AUDIT_LOG_USER_UserId] FOREIGN KEY ([UserId]) REFERENCES [USER] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [IDENTITY_USER_CLAIM] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_IDENTITY_USER_CLAIM] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IDENTITY_USER_CLAIM_USER_UserId] FOREIGN KEY ([UserId]) REFERENCES [USER] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [IDENTITY_USER_LOGIN] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_IDENTITY_USER_LOGIN] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_IDENTITY_USER_LOGIN_USER_UserId] FOREIGN KEY ([UserId]) REFERENCES [USER] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [IDENTITY_USER_ROLE] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_IDENTITY_USER_ROLE] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_IDENTITY_USER_ROLE_IDENTITY_ROLE_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [IDENTITY_ROLE] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_IDENTITY_USER_ROLE_USER_UserId] FOREIGN KEY ([UserId]) REFERENCES [USER] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [IDENTITY_USER_TOKEN] (
        [UserId] int NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_IDENTITY_USER_TOKEN] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_IDENTITY_USER_TOKEN_USER_UserId] FOREIGN KEY ([UserId]) REFERENCES [USER] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [SEAT] (
        [Id] uniqueidentifier NOT NULL,
        [SectorId] int NOT NULL,
        [RowIdentifier] nvarchar(10) NOT NULL,
        [SeatNumber] int NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [Version] int NOT NULL,
        CONSTRAINT [PK_SEAT] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SEAT_SECTOR_SectorId] FOREIGN KEY ([SectorId]) REFERENCES [SECTOR] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE TABLE [RESERVATION] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] int NOT NULL,
        [SeatId] uniqueidentifier NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [ReservedAt] datetime2 NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        CONSTRAINT [PK_RESERVATION] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RESERVATION_SEAT_SeatId] FOREIGN KEY ([SeatId]) REFERENCES [SEAT] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RESERVATION_USER_UserId] FOREIGN KEY ([UserId]) REFERENCES [USER] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'EventDate', N'ImageUrl', N'Name', N'Status', N'Venue') AND [object_id] = OBJECT_ID(N'[EVENT]'))
        SET IDENTITY_INSERT [EVENT] ON;
    EXEC(N'INSERT INTO [EVENT] ([Id], [Description], [EventDate], [ImageUrl], [Name], [Status], [Venue])
    VALUES (1, NULL, ''2026-07-15T21:00:00.0000000'', NULL, N''Noches en Vivo 2026'', N''Published'', N''Microestadio UNAJ'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'EventDate', N'ImageUrl', N'Name', N'Status', N'Venue') AND [object_id] = OBJECT_ID(N'[EVENT]'))
        SET IDENTITY_INSERT [EVENT] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Capacity', N'EventId', N'Name', N'Price') AND [object_id] = OBJECT_ID(N'[SECTOR]'))
        SET IDENTITY_INSERT [SECTOR] ON;
    EXEC(N'INSERT INTO [SECTOR] ([Id], [Capacity], [EventId], [Name], [Price])
    VALUES (1, 50, 1, N''Campo'', 12000.0),
    (2, 50, 1, N''Platea'', 18000.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Capacity', N'EventId', N'Name', N'Price') AND [object_id] = OBJECT_ID(N'[SECTOR]'))
        SET IDENTITY_INSERT [SECTOR] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RowIdentifier', N'SeatNumber', N'SectorId', N'Status', N'Version') AND [object_id] = OBJECT_ID(N'[SEAT]'))
        SET IDENTITY_INSERT [SEAT] ON;
    EXEC(N'INSERT INTO [SEAT] ([Id], [RowIdentifier], [SeatNumber], [SectorId], [Status], [Version])
    VALUES (''00000000-0000-0000-0000-010000000001'', N''A'', 1, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000002'', N''A'', 2, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000003'', N''A'', 3, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000004'', N''A'', 4, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000005'', N''A'', 5, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000006'', N''A'', 6, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000007'', N''A'', 7, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000008'', N''A'', 8, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000009'', N''A'', 9, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000010'', N''A'', 10, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000011'', N''B'', 1, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000012'', N''B'', 2, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000013'', N''B'', 3, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000014'', N''B'', 4, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000015'', N''B'', 5, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000016'', N''B'', 6, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000017'', N''B'', 7, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000018'', N''B'', 8, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000019'', N''B'', 9, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000020'', N''B'', 10, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000021'', N''C'', 1, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000022'', N''C'', 2, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000023'', N''C'', 3, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000024'', N''C'', 4, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000025'', N''C'', 5, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000026'', N''C'', 6, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000027'', N''C'', 7, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000028'', N''C'', 8, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000029'', N''C'', 9, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000030'', N''C'', 10, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000031'', N''D'', 1, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000032'', N''D'', 2, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000033'', N''D'', 3, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000034'', N''D'', 4, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000035'', N''D'', 5, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000036'', N''D'', 6, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000037'', N''D'', 7, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000038'', N''D'', 8, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000039'', N''D'', 9, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000040'', N''D'', 10, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000041'', N''E'', 1, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000042'', N''E'', 2, 1, N''Available'', 1);
    INSERT INTO [SEAT] ([Id], [RowIdentifier], [SeatNumber], [SectorId], [Status], [Version])
    VALUES (''00000000-0000-0000-0000-010000000043'', N''E'', 3, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000044'', N''E'', 4, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000045'', N''E'', 5, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000046'', N''E'', 6, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000047'', N''E'', 7, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000048'', N''E'', 8, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000049'', N''E'', 9, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-010000000050'', N''E'', 10, 1, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000001'', N''A'', 1, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000002'', N''A'', 2, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000003'', N''A'', 3, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000004'', N''A'', 4, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000005'', N''A'', 5, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000006'', N''A'', 6, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000007'', N''A'', 7, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000008'', N''A'', 8, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000009'', N''A'', 9, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000010'', N''A'', 10, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000011'', N''B'', 1, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000012'', N''B'', 2, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000013'', N''B'', 3, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000014'', N''B'', 4, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000015'', N''B'', 5, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000016'', N''B'', 6, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000017'', N''B'', 7, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000018'', N''B'', 8, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000019'', N''B'', 9, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000020'', N''B'', 10, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000021'', N''C'', 1, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000022'', N''C'', 2, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000023'', N''C'', 3, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000024'', N''C'', 4, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000025'', N''C'', 5, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000026'', N''C'', 6, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000027'', N''C'', 7, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000028'', N''C'', 8, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000029'', N''C'', 9, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000030'', N''C'', 10, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000031'', N''D'', 1, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000032'', N''D'', 2, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000033'', N''D'', 3, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000034'', N''D'', 4, 2, N''Available'', 1);
    INSERT INTO [SEAT] ([Id], [RowIdentifier], [SeatNumber], [SectorId], [Status], [Version])
    VALUES (''00000000-0000-0000-0000-020000000035'', N''D'', 5, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000036'', N''D'', 6, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000037'', N''D'', 7, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000038'', N''D'', 8, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000039'', N''D'', 9, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000040'', N''D'', 10, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000041'', N''E'', 1, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000042'', N''E'', 2, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000043'', N''E'', 3, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000044'', N''E'', 4, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000045'', N''E'', 5, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000046'', N''E'', 6, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000047'', N''E'', 7, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000048'', N''E'', 8, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000049'', N''E'', 9, 2, N''Available'', 1),
    (''00000000-0000-0000-0000-020000000050'', N''E'', 10, 2, N''Available'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RowIdentifier', N'SeatNumber', N'SectorId', N'Status', N'Version') AND [object_id] = OBJECT_ID(N'[SEAT]'))
        SET IDENTITY_INSERT [SEAT] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AUDIT_LOG_UserId] ON [AUDIT_LOG] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [IDENTITY_ROLE] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_IDENTITY_ROLE_CLAIM_RoleId] ON [IDENTITY_ROLE_CLAIM] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_IDENTITY_USER_CLAIM_UserId] ON [IDENTITY_USER_CLAIM] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_IDENTITY_USER_LOGIN_UserId] ON [IDENTITY_USER_LOGIN] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_IDENTITY_USER_ROLE_RoleId] ON [IDENTITY_USER_ROLE] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RESERVATION_SeatId] ON [RESERVATION] ([SeatId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RESERVATION_UserId] ON [RESERVATION] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_SEAT_SectorId_RowIdentifier_SeatNumber] ON [SEAT] ([SectorId], [RowIdentifier], [SeatNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SECTOR_EventId] ON [SECTOR] ([EventId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [USER] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_USER_Email] ON [USER] ([Email]) WHERE [Email] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [USER] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260514213906_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260514213906_InitialCreate', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625004433_AddRefreshTokenToUsers'
)
BEGIN
    ALTER TABLE [USER] ADD [RefreshTokenExpiresAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625004433_AddRefreshTokenToUsers'
)
BEGIN
    ALTER TABLE [USER] ADD [RefreshTokenHash] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625004433_AddRefreshTokenToUsers'
)
BEGIN
    CREATE INDEX [IX_USER_RefreshTokenHash] ON [USER] ([RefreshTokenHash]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625004433_AddRefreshTokenToUsers'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625004433_AddRefreshTokenToUsers', N'8.0.8');
END;
GO

COMMIT;
GO

