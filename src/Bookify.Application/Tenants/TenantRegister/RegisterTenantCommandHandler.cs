using System.Net;
using Bookify.Application.Abstractions.Authentication.Register;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Errors;
using Bookify.Domain.Tenants;
using Bookify.Domain.Tenants.ValueObjects;
using Bookify.Domain.Utility.Results;

namespace Bookify.Application.Tenants.TenantRegister;

public sealed class RegisterTenantCommandHandler : ICommandHandler<RegisterTenantCommand, Guid>
{
    private readonly IAuthRegistrationService _authRegistrationService;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterTenantCommandHandler(
        IAuthRegistrationService authRegistrationService,
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork)
    {
        _authRegistrationService = authRegistrationService;
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterTenantCommand request, CancellationToken cancellationToken)
    {
        var registrationRequest = new RegistrationRequest(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var registrationResponse = await _authRegistrationService.RegisterAsync(registrationRequest, cancellationToken);
        if(registrationResponse is null) return DomainErrors.Tenant.RegistrationFailed;
        
        var newTenant = Tenant.Create(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email),
            registrationResponse.AuthenticationId);
        
        await _tenantRepository.AddAsync(newTenant);
        await _unitOfWork.CommitAsync(cancellationToken);

        return newTenant.Id.Value;
    }
}