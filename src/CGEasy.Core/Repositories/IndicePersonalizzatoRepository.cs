using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

public class IndicePersonalizzatoRepository
{
    private readonly LiteDbContext _context;

    public IndicePersonalizzatoRepository(LiteDbContext context)
    {
        _context = context;
    }

    public List<IndicePersonalizzato> GetAll()
    {
        return _context.IndiciPersonalizzati.FindAll().ToList();
    }

    public List<IndicePersonalizzato> GetByCliente(int clienteId)
    {
        return _context.IndiciPersonalizzati
            .Find(i => i.ClienteId == clienteId && i.Attivo)
            .OrderBy(i => i.NomeIndice)
            .ToList();
    }

    public IndicePersonalizzato? GetById(int id)
    {
        return _context.IndiciPersonalizzati.FindById(id);
    }

    public IndicePersonalizzato Insert(IndicePersonalizzato indice)
    {
        indice.DataCreazione = DateTime.Now;
        _context.IndiciPersonalizzati.Insert(indice);
        return indice;
    }

    public bool Update(IndicePersonalizzato indice)
    {
        indice.DataUltimaModifica = DateTime.Now;
        return _context.IndiciPersonalizzati.Update(indice);
    }

    public bool Delete(int id)
    {
        return _context.IndiciPersonalizzati.Delete(id);
    }

    public bool Exists(int clienteId, string nomeIndice, int? excludeId = null)
    {
        var query = _context.IndiciPersonalizzati.Find(i => 
            i.ClienteId == clienteId && 
            i.NomeIndice.ToLower() == nomeIndice.ToLower() &&
            i.Attivo);

        if (excludeId.HasValue)
        {
            query = query.Where(i => i.Id != excludeId.Value);
        }

        return query.Any();
    }
}

