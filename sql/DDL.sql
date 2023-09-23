CREATE DATABASE [Bookify];

CREATE TABLE [Apartment](
	[ApartmentId] CHAR(36),
	[Name] VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[AddressCountry] VARCHAR(255) NOT NULL,
	[AddressState] VARCHAR(255) NOT NULL,
	[AddressCity] VARCHAR(255) NOT NULL,
	[AddressStreet] VARCHAR(255) NOT NULL,
	[AddressNumber] VARCHAR(255) NOT NULL,
	[AddressPostalCode] VARCHAR(255) NOT NULL,
	[PriceAmount] DECIMAL(18,2) NOT NULL,
	[PriceCurrencyCode] INTEGER NOT NULL,
	[CleaningFeeAmount] DECIMAL(18,2) NOT NULL,
	[CleaningFeeCurrencyCode] INTEGER NOT NULL,
	[LastBookedOnUtc] DATETIME NULL,

	CONSTRAINT PK_Apartment_ApartmentId PRIMARY KEY ([ApartmentId]),
	CONSTRAINT CK_Apartment_PriceAmount CHECK ([PriceAmount] >= 0),
	CONSTRAINT CK_Apartment_CleaningFeeAmount CHECK ([CleaningFeeAmount] >= 0),
	CONSTRAINT CK_Apartment_LastBookedOnUtc CHECK ([LastBookedOnUtc] IS NULL OR [LastBookedOnUtc] <= GETUTCDATE())
);

CREATE TABLE [ApartmentAmenity](
	[ApartmentAmenityId] CHAR(36),
	[ApartmentId] CHAR(36) NOT NULL,
	[AmenityCode] INTEGER NOT NULL,

	CONSTRAINT PK_ApartmentAmenity_ApartmentAmenityId PRIMARY KEY ([ApartmentAmenityId]),
	CONSTRAINT FK_ApartmentAmenity_Apartment_ApartmentId 
		FOREIGN KEY ([ApartmentId]) REFERENCES [Apartment] ([ApartmentId]),
);

CREATE TABLE [Tenant](
	[TenantId] CHAR(36),
	[FirstName] VARCHAR(255) NOT NULL,
	[LastName] VARCHAR(255) NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
	[AuthenticationId] VARCHAR(200) NOT NULL,

	CONSTRAINT PK_Tenant_TenantId PRIMARY KEY ([TenantId]),
	CONSTRAINT CK_Tenant_Email CHECK ([Email] LIKE '%@%.%')
);

CREATE TABLE [Booking](
	[BookingId] CHAR(36),
	[ApartmentId] CHAR(36) NOT NULL,
	[TenantId] CHAR(36) NOT NULL,
	[DurationStartUtc] DATETIME NOT NULL,
	[DurationEndUtc] DATETIME NOT NULL,
	[PricePerPeriodAmount] DECIMAL(18,2) NOT NULL,
	[PricePerPeriodCurrencyCode] INTEGER NOT NULL,
	[CleaningFeeAmount] DECIMAL(18,2) NOT NULL,
	[CleaningFeeCurrencyCode] INTEGER NOT NULL,
	[AmenitiesUpChargeAmount] DECIMAL(18,2) NOT NULL,
	[AmenitiesUpChargeCurrencyCode] INTEGER NOT NULL,
	[TotalPriceAmount] DECIMAL(18,2) NOT NULL,
	[TotalPriceCurrencyCode] INTEGER NOT NULL,
	[Status] INTEGER NOT NULL,
	[CreatedOnUtc] DATETIME NOT NULL,
	[ConfirmedOnUtc] DATETIME NULL,
	[RejectedOnUtc] DATETIME NULL,
	[CompletedOnUtc] DATETIME NULL,
	[CancelledOnUtc] DATETIME NULL,

	CONSTRAINT PK_Booking_BookingId PRIMARY KEY ([BookingId]),
	CONSTRAINT FK_Booking_Apartment_ApartmentId 
		FOREIGN KEY ([ApartmentId]) REFERENCES [Apartment] ([ApartmentId]),
	CONSTRAINT FK_Booking_Tenant_TenantId
		FOREIGN KEY ([TenantId]) REFERENCES [Tenant] ([TenantId]),
	CONSTRAINT CK_Booking_Duration CHECK ([DurationStartUtc] < [DurationEndUtc]),
	CONSTRAINT CK_Booking_PricePerPeriodAmount CHECK ([PricePerPeriodAmount] >= 0),
	CONSTRAINT CK_Booking_CleaningFeeAmount CHECK ([CleaningFeeAmount] >= 0),
	CONSTRAINT CK_Booking_AmenitiesUpChargeAmount CHECK ([AmenitiesUpChargeAmount] >= 0),
	CONSTRAINT CK_Booking_TotalPriceAmount CHECK ([TotalPriceAmount] >= 0)
);


