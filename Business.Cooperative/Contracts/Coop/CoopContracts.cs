using Model.Cooperative;

namespace Business.Cooperative.Contracts.Coop;

public sealed record CoopDto(int IdCoop, string CoopName, decimal Budget);

public sealed record CreateCoopRequest(string CoopName, decimal Budget);

public sealed record UpdateCoopRequest(string CoopName, decimal Budget);

public sealed record CreateCoopWithOwnerRequest(
    string CoopName,
    decimal Budget,
    string FirstName,
    string LastName,
    decimal FeesPerYear,
    ApplicationUser User);
