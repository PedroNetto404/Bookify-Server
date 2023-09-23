using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Errors;

public static partial class DomainErrors
{
    public static class Booking
    {
        public static  Error NotFound => new("Booking.NotFound", "Booking not found.");    
    }

    public class Tenant
    {
        public static Error NotFound => new("Tenant.NotFound", "Tenant not found.");
        
        public static Error RegistrationFailed => new("Tenant.RegistrationFailed", "Tenant registration failed.");
    }
}