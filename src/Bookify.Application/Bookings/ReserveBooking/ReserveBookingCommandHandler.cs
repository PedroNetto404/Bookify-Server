using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Abstractions;
using Bookify.Domain.Bookings.Services;
using Bookify.Domain.Bookings.ValueObjects;
using Bookify.Domain.Tenants;
using Bookify.Domain.Tenants.ValueObjects;
using Bookify.Domain.Utility.Results;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly PricingService _pricingService;

    public ReserveBookingCommandHandler(
        IApartmentRepository apartmentRepository,
        ITenantRepository tenantRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        PricingService pricingService)
    {
        _apartmentRepository = apartmentRepository;
        _tenantRepository = tenantRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _pricingService = pricingService;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {   
        var tenantId = new TenantId(request.TenantId);
        
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant is null) return BookingErrors.NotFound;
        
        var apartment = await _apartmentRepository.GetByIdAsync(new ApartmentId(request.ApartmentId));
        if (apartment is null) return BookingErrors.NotFound;

        var durationResult = DateRange.Create(request.StartDate, request.EndDate);
        if (durationResult is not { IsSuccess: true, Value: { } duration })
            return durationResult.Error;

        if(await _bookingRepository.IsOverlappingAsync(apartment, duration))
            return BookingErrors.Overlap;

        var booking = Booking.Reserve(
            apartment,
            tenantId,
            duration,
            _dateTimeProvider.UtcNow,
            _pricingService);

        await _bookingRepository.AddAsync(booking);
        await _unitOfWork.CommitAsync(cancellationToken);

        return booking.Id.Value;
    }
}