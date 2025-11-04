using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

/// <summary>
/// Repository per gestione dello storico Saldi Giornalieri delle Banche
/// </summary>
public class BancaSaldoGiornalieroRepository
{
    private readonly LiteDbContext _context;

    public BancaSaldoGiornalieroRepository(LiteDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera tutti i saldi giornalieri
    /// </summary>
    public List<BancaSaldoGiornaliero> GetAll()
    {
        return _context.BancaSaldoGiornaliero.FindAll().ToList();
    }

    /// <summary>
    /// Recupera un saldo giornaliero per ID
    /// </summary>
    public BancaSaldoGiornaliero? GetById(int id)
    {
        return _context.BancaSaldoGiornaliero.FindById(id);
    }

    /// <summary>
    /// Recupera tutti i saldi giornalieri di una banca
    /// </summary>
    public List<BancaSaldoGiornaliero> GetByBancaId(int bancaId)
    {
        return _context.BancaSaldoGiornaliero
            .Find(s => s.BancaId == bancaId)
            .OrderBy(s => s.Data)
            .ToList();
    }

    /// <summary>
    /// Recupera il saldo alla data specificata (o il più recente precedente)
    /// </summary>
    public BancaSaldoGiornaliero? GetAllaData(int bancaId, DateTime data)
    {
        return _context.BancaSaldoGiornaliero
            .Find(s => s.BancaId == bancaId && s.Data <= data)
            .OrderByDescending(s => s.Data)
            .FirstOrDefault();
    }

    /// <summary>
    /// Recupera i saldi in un range di date
    /// </summary>
    public List<BancaSaldoGiornaliero> GetByRange(int bancaId, DateTime dataInizio, DateTime dataFine)
    {
        return _context.BancaSaldoGiornaliero
            .Find(s => s.BancaId == bancaId && s.Data >= dataInizio && s.Data <= dataFine)
            .OrderBy(s => s.Data)
            .ToList();
    }

    /// <summary>
    /// Inserisce un nuovo saldo giornaliero
    /// </summary>
    public int Insert(BancaSaldoGiornaliero saldo)
    {
        saldo.DataCreazione = DateTime.Now;
        return _context.BancaSaldoGiornaliero.Insert(saldo);
    }

    /// <summary>
    /// Aggiorna un saldo giornaliero esistente
    /// </summary>
    public bool Update(BancaSaldoGiornaliero saldo)
    {
        return _context.BancaSaldoGiornaliero.Update(saldo);
    }

    /// <summary>
    /// Elimina un saldo giornaliero
    /// </summary>
    public bool Delete(int id)
    {
        return _context.BancaSaldoGiornaliero.Delete(id);
    }

    /// <summary>
    /// Elimina tutti i saldi giornalieri di una banca
    /// </summary>
    public int DeleteByBancaId(int bancaId)
    {
        return _context.BancaSaldoGiornaliero.DeleteMany(s => s.BancaId == bancaId);
    }

    /// <summary>
    /// Salva/aggiorna il saldo alla data specificata
    /// Se esiste già un record per quella data, lo aggiorna; altrimenti lo crea
    /// </summary>
    public bool SaveSaldoAllaData(int bancaId, DateTime data, decimal saldo, string? note = null)
    {
        // Cerca un record esistente per questa data
        var esistente = _context.BancaSaldoGiornaliero
            .FindOne(s => s.BancaId == bancaId && s.Data.Date == data.Date);

        if (esistente != null)
        {
            // Aggiorna il record esistente
            esistente.Saldo = saldo;
            esistente.Note = note;
            return Update(esistente);
        }
        else
        {
            // Crea un nuovo record
            var nuovo = new BancaSaldoGiornaliero
            {
                BancaId = bancaId,
                Data = data.Date,
                Saldo = saldo,
                Note = note
            };
            Insert(nuovo);
            return true;
        }
    }
}

