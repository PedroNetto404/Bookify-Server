using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Abstractions.Messaging.Clock;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly PricingService _pricingService;

    public ReserveBookingCommandHandler(
        IApartmentRepository apartmentRepository,
        IUserRepository userRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        PricingService pricingService)
    {
        _apartmentRepository = apartmentRepository;
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _pricingService = pricingService;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {   
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if(user is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId);
        if(apartment is null)
        {
            return Result.Failure<Guid>(BookingErrors.NotFound);
        }

        var durationResult = DateRange.Create(request.StartDate, request.EndDate);
        if(durationResult is not { IsSuccess: true, Value: {} duration })
        {
            return Result.Failure<Guid>(durationResult.Error);
        }

        if(await _bookingRepository.IsOverlappingAsync(apartment, duration))
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        var booking = Booking.Reserve(
            apartment,
            request.UserId,
            duration,
            _dateTimeProvider.Now,
            _pricingService);

        _bookingRepository.Add(booking);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(booking.Identifier);
    }
}