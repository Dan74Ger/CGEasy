using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

/// <summary>
/// Repository per gestione CRUD delle Banche
/// </summary>
public class BancaRepository
{
    private readonly LiteDbContext _context;

    public BancaRepository(LiteDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera tutte le banche
    /// </summary>
    public List<Banca> GetAll()
    {
        return _context.Banche.FindAll().ToList();
    }

    /// <summary>
    /// Recupera una banca per ID
    /// </summary>
    public Banca? GetById(int id)
    {
        return _context.Banche.FindById(id);
    }

    /// <summary>
    /// Cerca banche per nome
    /// </summary>
    public List<Banca> SearchByNome(string nome)
    {
        return _context.Banche
            .Find(b => b.NomeBanca.Contains(nome))
            .ToList();
    }

    /// <summary>
    /// Inserisce una nuova banca
    /// </summary>
    public int Insert(Banca banca)
    {
        banca.DataCreazione = DateTime.Now;
        banca.DataUltimaModifica = DateTime.Now;
        return _context.Banche.Insert(banca);
    }

    /// <summary>
    /// Aggiorna una banca esistente
    /// </summary>
    public bool Update(Banca banca)
    {
        banca.DataUltimaModifica = DateTime.Now;
        return _context.Banche.Update(banca);
    }

    /// <summary>
    /// Elimina una banca
    /// </summary>
    public bool Delete(int id)
    {
        return _context.Banche.Delete(id);
    }

    /// <summary>
    /// Conta il numero totale di banche
    /// </summary>
    public int Count()
    {
        return _context.Banche.Count();
    }

    /// <summary>
    /// Aggiorna il saldo del giorno per una banca
    /// </summary>
    public bool UpdateSaldoDelGiorno(int bancaId, decimal nuovoSaldo)
    {
        var banca = GetById(bancaId);
        if (banca == null) return false;

        banca.SaldoDelGiorno = nuovoSaldo;
        return Update(banca);
    }
}

