using Business.Cooperative.Contracts.Coop;

namespace Business.Cooperative.Interfaces;

public interface ICoopService
{
    Task<IReadOnlyList<CoopDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CoopDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CoopDto> CreateAsync(CreateCoopRequest request, CancellationToken cancellationToken = default);
    Task<CoopDto?> UpdateAsync(int id, UpdateCoopRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<CoopDto> CreateWithOwnerAsync(CreateCoopWithOwnerRequest request, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
