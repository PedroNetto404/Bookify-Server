CREATE DATABASE [Bookify];

CREATE TABLE [Apartment](
	[ApartmentId] CHAR(36) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[AddressCountry] VARCHAR(255) NOT NULL,
	[AddressCity] VARCHAR(255) NOT NULL,
	[AddressStreet] VARCHAR(255) NOT NULL,
	[AddressNumber] VARCHAR(255) NOT NULL,
	[AddressPostalCode] VARCHAR(255) NOT NULL,
	[PriceAmount] DECIMAL(18,2) NOT NULL,
	[PriceCurrency] INTEGER NOT NULL,
	[CleaningFeeAmount] DECIMAL(18,2) NOT NULL,
	[CleaningFeeCurrency] INTEGER NOT NULL,
	[LastBookedOnUtc] DATETIME NULL,

	CONSTRAINT PK_Apartment_ApartmentId PRIMARY KEY ([ApartmentId]),
	CONSTRAINT CK_Apartment_PriceAmount CHECK ([PriceAmount] >= 0),
	CONSTRAINT CK_Apartment_CleaningFeeAmount CHECK ([CleaningFeeAmount] >= 0),
	CONSTRAINT CK_Apartment_LastBookedOnUtc CHECK ([LastBookedOnUtc] IS NULL OR [LastBookedOnUtc] <= GETUTCDATE())
);

CREATE TABLE [ApartmentAmenity](
	[ApartmentAmenityId] CHAR(36) NOT NULL,
	[ApartmentId] CHAR(36) NOT NULL,
	[AmenityCode] INTEGER NOT NULL,

	CONSTRAINT PK_ApartmentAmenity_ApartmentAmenityId PRIMARY KEY ([ApartmentAmenityId]),
	CONSTRAINT FK_ApartmentAmenity_Apartment_ApartmentId 
		FOREIGN KEY ([ApartmentId]) REFERENCES [Apartment] ([ApartmentId]),
);

-- todo: CREATE TABLE Tenant