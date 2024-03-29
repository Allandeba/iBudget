﻿using System.Linq.Expressions;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Repository;

public class LoginRepository : IRepository<LoginModel>
{
    private readonly ApplicationDBContext _context;

    public LoginRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoginModel login)
    {
        _ = await _context.Login.AddAsync(login);
        _ = await _context.SaveChangesAsync();
    }

    public Task<LoginModel> GetByIdAsync(int id, Enum[] includes)
    {
        throw new NotImplementedException();
    }

    public IQueryable<LoginModel> GetAll(Enum[] includes)
    {
        return _context.Login.AsNoTracking();
    }

    public IQueryable<LoginModel> Find(Expression<Func<LoginModel, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LoginModel entity)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(LoginModel entity)
    {
        throw new NotImplementedException();
    }

    public async Task<LoginModel> GetByUsernameAsync(string username)
    {
        return await _context.Login.FirstOrDefaultAsync(l => l.Username.Equals(username));
    }

    public async Task SaveLoginLog(LoginLogModel loginLog)
    {
        _ = await _context.LoginLog.AddAsync(loginLog);
        _ = await _context.SaveChangesAsync();
    }
}