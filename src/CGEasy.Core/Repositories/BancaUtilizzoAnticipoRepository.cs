using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

/// <summary>
/// Repository per gestione CRUD dell'Utilizzo Anticipo Fatture/SBF per Banca
/// </summary>
public class BancaUtilizzoAnticipoRepository
{
    private readonly LiteDbContext _context;

    public BancaUtilizzoAnticipoRepository(LiteDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera tutti gli utilizzi anticipo
    /// </summary>
    public List<BancaUtilizzoAnticipo> GetAll()
    {
        return _context.BancaUtilizzoAnticipo.FindAll().ToList();
    }

    /// <summary>
    /// Recupera un utilizzo anticipo per ID
    /// </summary>
    public BancaUtilizzoAnticipo? GetById(int id)
    {
        return _context.BancaUtilizzoAnticipo.FindById(id);
    }

    /// <summary>
    /// Recupera tutti gli utilizzi anticipo di una banca
    /// </summary>
    public List<BancaUtilizzoAnticipo> GetByBancaId(int bancaId)
    {
        return _context.BancaUtilizzoAnticipo
            .Find(u => u.BancaId == bancaId)
            .OrderBy(u => u.DataInizioUtilizzo)
            .ToList();
    }

    /// <summary>
    /// Recupera gli utilizzi anticipo attivi (non ancora rimborsati)
    /// </summary>
    public List<BancaUtilizzoAnticipo> GetAttivi(int bancaId)
    {
        return _context.BancaUtilizzoAnticipo
            .Find(u => u.BancaId == bancaId && !u.Rimborsato)
            .OrderBy(u => u.DataInizioUtilizzo)
            .ToList();
    }

    /// <summary>
    /// Recupera gli utilizzi in scadenza entro una certa data
    /// </summary>
    public List<BancaUtilizzoAnticipo> GetInScadenzaEntro(int bancaId, DateTime dataLimite)
    {
        return _context.BancaUtilizzoAnticipo
            .Find(u => u.BancaId == bancaId && !u.Rimborsato && u.DataScadenzaUtilizzo <= dataLimite)
            .OrderBy(u => u.DataScadenzaUtilizzo)
            .ToList();
    }

    /// <summary>
    /// Inserisce un nuovo utilizzo anticipo
    /// </summary>
    public int Insert(BancaUtilizzoAnticipo utilizzo)
    {
        utilizzo.DataCreazione = DateTime.Now;
        utilizzo.DataUltimaModifica = DateTime.Now;
        return _context.BancaUtilizzoAnticipo.Insert(utilizzo);
    }

    /// <summary>
    /// Aggiorna un utilizzo anticipo esistente
    /// </summary>
    public bool Update(BancaUtilizzoAnticipo utilizzo)
    {
        utilizzo.DataUltimaModifica = DateTime.Now;
        return _context.BancaUtilizzoAnticipo.Update(utilizzo);
    }

    /// <summary>
    /// Elimina un utilizzo anticipo
    /// </summary>
    public bool Delete(int id)
    {
        return _context.BancaUtilizzoAnticipo.Delete(id);
    }

    /// <summary>
    /// Elimina tutti gli utilizzi anticipo di una banca
    /// </summary>
    public int DeleteByBancaId(int bancaId)
    {
        return _context.BancaUtilizzoAnticipo.DeleteMany(u => u.BancaId == bancaId);
    }

    /// <summary>
    /// Segna un utilizzo anticipo come rimborsato
    /// </summary>
    public bool SegnaRimborsato(int id, DateTime dataRimborsoEffettivo)
    {
        var utilizzo = GetById(id);
        if (utilizzo == null) return false;

        utilizzo.Rimborsato = true;
        utilizzo.DataRimborsoEffettivo = dataRimborsoEffettivo;
        return Update(utilizzo);
    }

    /// <summary>
    /// Calcola il totale degli utilizzi attivi per una banca
    /// </summary>
    public decimal GetTotaleUtilizziAttivi(int bancaId)
    {
        return _context.BancaUtilizzoAnticipo
            .Find(u => u.BancaId == bancaId && !u.Rimborsato)
            .Sum(u => u.ImportoUtilizzo);
    }
}

