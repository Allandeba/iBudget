using System.Linq.Expressions;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Framework;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Repository;

public class PersonRepository : IRepository<PersonModel>
{
    private readonly ApplicationDBContext _context;

    public PersonRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<PersonModel> GetByIdAsync(int PersonId, Enum[] includes)
    {
        var query = GetQuery(includes);
        return await query.FirstOrDefaultAsync(p => p.PersonId == PersonId);
    }

    public IQueryable<PersonModel> GetAll(Enum[] includes)
    {
        var query = GetQuery(includes);
        return query.AsNoTracking();
    }

    public IQueryable<PersonModel> Find(Expression<Func<PersonModel, bool>> where)
    {
        return _context.Person.AsNoTracking().Where(where);
    }

    public async Task AddAsync(PersonModel person)
    {
        _ = await _context.Person.AddAsync(person);
        _ = await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PersonModel person)
    {
        _ = _context.Person.Update(person);
        _ = await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(PersonModel person)
    {
        _ = _context.Person.Remove(person);
        _ = await _context.SaveChangesAsync();
    }

    private IQueryable<PersonModel> GetQuery(Enum[] includes)
    {
        IQueryable<PersonModel> query = _context.Person;
        foreach (PersonIncludes include in includes)
            switch (include)
            {
                case PersonIncludes.None:
                    break;

                case PersonIncludes.Contact:
                    query = query.Include(c => c.Contact);
                    break;

                case PersonIncludes.Document:
                    query = query.Include(d => d.Document);
                    break;

                default:
                    throw new Exception("PersonIncludes type not implemented");
            }

        return query;
    }
}