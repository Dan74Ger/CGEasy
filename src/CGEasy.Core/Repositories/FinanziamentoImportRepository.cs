using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

public class FinanziamentoImportRepository
{
    private readonly LiteDbContext _context;

    public FinanziamentoImportRepository(LiteDbContext context)
    {
        _context = context;
    }

    public int Insert(FinanziamentoImport finanziamento)
    {
        finanziamento.DataCreazione = DateTime.Now;
        var id = _context.FinanziamentoImport.Insert(finanziamento);
        _context.Checkpoint();
        return id.AsInt32;
    }

    public bool Update(FinanziamentoImport finanziamento)
    {
        var result = _context.FinanziamentoImport.Update(finanziamento);
        _context.Checkpoint();
        return result;
    }

    public bool Delete(int id)
    {
        var result = _context.FinanziamentoImport.Delete(id);
        _context.Checkpoint();
        return result;
    }

    public FinanziamentoImport? GetById(int id)
    {
        return _context.FinanziamentoImport.FindById(id);
    }

    public List<FinanziamentoImport> GetAll()
    {
        return _context.FinanziamentoImport
            .FindAll()
            .OrderByDescending(f => f.DataCreazione)
            .ToList();
    }

    public List<FinanziamentoImport> GetByBancaId(int bancaId)
    {
        return _context.FinanziamentoImport
            .Find(f => f.BancaId == bancaId)
            .OrderByDescending(f => f.DataCreazione)
            .ToList();
    }

    public List<FinanziamentoImport> GetByUtenteId(int utenteId)
    {
        return _context.FinanziamentoImport
            .Find(f => f.UtenteId == utenteId)
            .OrderByDescending(f => f.DataCreazione)
            .ToList();
    }

    public FinanziamentoImport? GetByIncassoId(int incassoId)
    {
        return _context.FinanziamentoImport
            .FindOne(f => f.IncassoId == incassoId);
    }

    public FinanziamentoImport? GetByPagamentoId(int pagamentoId)
    {
        return _context.FinanziamentoImport
            .FindOne(f => f.PagamentoId == pagamentoId);
    }
}

