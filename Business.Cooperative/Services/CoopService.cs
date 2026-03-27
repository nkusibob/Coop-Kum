using Business.Cooperative.Contracts.Coop;
using Business.Cooperative.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;

namespace Business.Cooperative.Services;

public class CoopService : ICoopService
{
    private readonly CooperativeContext _context;

    public CoopService(CooperativeContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CoopDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Coop
            .AsNoTracking()
            .Select(c => new CoopDto(c.IdCoop, c.CoopName, c.Budget))
            .ToListAsync(cancellationToken);
    }

    public async Task<CoopDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Coop
            .AsNoTracking()
            .Where(c => c.IdCoop == id)
            .Select(c => new CoopDto(c.IdCoop, c.CoopName, c.Budget))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CoopDto> CreateAsync(CreateCoopRequest request, CancellationToken cancellationToken = default)
    {
        var coop = new Coop
        {
            CoopName = request.CoopName,
            Budget = request.Budget
        };

        _context.Coop.Add(coop);
        await _context.SaveChangesAsync(cancellationToken);

        return new CoopDto(coop.IdCoop, coop.CoopName, coop.Budget);
    }

    public async Task<CoopDto?> UpdateAsync(int id, UpdateCoopRequest request, CancellationToken cancellationToken = default)
    {
        var coop = await _context.Coop.FirstOrDefaultAsync(c => c.IdCoop == id, cancellationToken);
        if (coop is null)
        {
            return null;
        }

        coop.CoopName = request.CoopName;
        coop.Budget = request.Budget;

        await _context.SaveChangesAsync(cancellationToken);
        return new CoopDto(coop.IdCoop, coop.CoopName, coop.Budget);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var coop = await _context.Coop.FirstOrDefaultAsync(c => c.IdCoop == id, cancellationToken);
        if (coop is null)
        {
            return false;
        }

        _context.Coop.Remove(coop);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<CoopDto> CreateWithOwnerAsync(CreateCoopWithOwnerRequest request, CancellationToken cancellationToken = default)
    {
        request.User.FirstName = request.FirstName;
        request.User.LastName = request.LastName;

        var coop = new Coop
        {
            CoopName = request.CoopName,
            Budget = request.Budget
        };

        var person = ConnectedMember.CreateFromUser(request.User);
        var membre = Membre.Create(person, coop, request.FeesPerYear);
        coop.Membres.Add(membre);

        _context.Coop.Add(coop);
        await _context.SaveChangesAsync(cancellationToken);

        return new CoopDto(coop.IdCoop, coop.CoopName, coop.Budget);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Coop.AnyAsync(c => c.IdCoop == id, cancellationToken);
    }
}
