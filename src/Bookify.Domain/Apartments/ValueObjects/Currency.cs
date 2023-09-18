namespace Bookify.Domain.Apartments;

/// <summary>
/// ISO 4217 currency code
/// </summary>
public record Currency
{
        
    public static readonly Currency USD = new("USD", 840);
    public static readonly Currency EUR = new("EUR", 978);
    public static readonly Currency BRL = new("BRL", 986);
    public static readonly Currency None = new("", 0); 

    private Currency(string code, int id) => (Code, Id) = (code, id);
    
    public string Code { get; init; }
    public int Id { get; init; }
    
    public static Currency FromCode(string code) => 
        All.Single(x => x.Code == code);

    public static Currency FromId(int id) => 
        All.Single(x => x.Id == id);

    public static readonly IReadOnlyCollection<Currency> All = new[]
    {
        USD,
        EUR,
        BRL
    };

    public static implicit operator Currency(string code) => FromCode(code);

    public static implicit operator Currency(int id) => FromId(id);
}