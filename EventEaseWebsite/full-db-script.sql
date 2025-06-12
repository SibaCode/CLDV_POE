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
CREATE TABLE [EventType] (
    [EventTypeId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_EventType] PRIMARY KEY ([EventTypeId])
);

CREATE TABLE [Venues] (
    [VenueId] int NOT NULL IDENTITY,
    [VenueName] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Capacity] int NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([VenueId])
);

CREATE TABLE [Events] (
    [EventId] int NOT NULL IDENTITY,
    [EventName] nvarchar(max) NOT NULL,
    [EventDate] datetime2 NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [VenueId] int NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [EventTypeId] int NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([EventId]),
    CONSTRAINT [FK_Events_EventType_EventTypeId] FOREIGN KEY ([EventTypeId]) REFERENCES [EventType] ([EventTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([VenueId]) ON DELETE NO ACTION
);

CREATE TABLE [Bookings] (
    [BookingId] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [VenueId] int NOT NULL,
    [BookingDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_Bookings_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([EventId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Bookings_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([VenueId]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Bookings_EventId] ON [Bookings] ([EventId]);

CREATE INDEX [IX_Bookings_VenueId] ON [Bookings] ([VenueId]);

CREATE INDEX [IX_Events_EventTypeId] ON [Events] ([EventTypeId]);

CREATE INDEX [IX_Events_VenueId] ON [Events] ([VenueId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250610164151_FixVenueModel', N'9.0.4');

COMMIT;
GO

