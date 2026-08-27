using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SanSaludAPI.DataAccess
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly SanSaludDbContext _context;

        public MedicoRepository(SanSaludDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            return await _context.Medicos.ToListAsync();
        }

        public async Task<Medico?> GetByIdAsync(Guid id)
        {
            return await _context.Medicos.FindAsync(id);
        }

        public async Task<Medico> CreateAsync(Medico medico)
        {
            medico.Id = Guid.NewGuid();
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            return medico;
        }
    }
}
