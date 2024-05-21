namespace TravelAgency.FleetService.API.Infrastructure.Interfaces;

public interface ICurrentUserService
{
    string? AccessToken { get; }
    string? Id { get; }
}
