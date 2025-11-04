using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

/// <summary>
/// Repository per gestione CRUD degli Incassi da Clienti per Banca
/// </summary>
public class BancaIncassoRepository
{
    private readonly LiteDbContext _context;

    public BancaIncassoRepository(LiteDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera tutti gli incassi
    /// </summary>
    public List<BancaIncasso> GetAll()
    {
        return _context.BancaIncassi.FindAll().ToList();
    }

    /// <summary>
    /// Recupera un incasso per ID
    /// </summary>
    public BancaIncasso? GetById(int id)
    {
        return _context.BancaIncassi.FindById(id);
    }

    /// <summary>
    /// Recupera tutti gli incassi di una banca
    /// </summary>
    public List<BancaIncasso> GetByBancaId(int bancaId)
    {
        return _context.BancaIncassi
            .Find(i => i.BancaId == bancaId)
            .OrderBy(i => i.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Recupera gli incassi per anno e mese
    /// </summary>
    public List<BancaIncasso> GetByPeriodo(int bancaId, int anno, int mese)
    {
        return _context.BancaIncassi
            .Find(i => i.BancaId == bancaId && i.Anno == anno && i.Mese == mese)
            .OrderBy(i => i.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Recupera gli incassi in scadenza entro una certa data
    /// </summary>
    public List<BancaIncasso> GetInScadenzaEntro(int bancaId, DateTime dataLimite)
    {
        return _context.BancaIncassi
            .Find(i => i.BancaId == bancaId && !i.Incassato && i.DataScadenza <= dataLimite)
            .OrderBy(i => i.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Recupera gli incassi non ancora incassati
    /// </summary>
    public List<BancaIncasso> GetNonIncassati(int bancaId)
    {
        return _context.BancaIncassi
            .Find(i => i.BancaId == bancaId && !i.Incassato)
            .OrderBy(i => i.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Inserisce un nuovo incasso
    /// </summary>
    public int Insert(BancaIncasso incasso)
    {
        incasso.DataCreazione = DateTime.Now;
        incasso.DataUltimaModifica = DateTime.Now;
        return _context.BancaIncassi.Insert(incasso);
    }

    /// <summary>
    /// Aggiorna un incasso esistente
    /// </summary>
    public bool Update(BancaIncasso incasso)
    {
        incasso.DataUltimaModifica = DateTime.Now;
        return _context.BancaIncassi.Update(incasso);
    }

    /// <summary>
    /// Elimina un incasso
    /// </summary>
    public bool Delete(int id)
    {
        return _context.BancaIncassi.Delete(id);
    }

    /// <summary>
    /// Elimina tutti gli incassi di una banca
    /// </summary>
    public int DeleteByBancaId(int bancaId)
    {
        return _context.BancaIncassi.DeleteMany(i => i.BancaId == bancaId);
    }

    /// <summary>
    /// Segna un incasso come incassato
    /// </summary>
    public bool SegnaIncassato(int id, DateTime dataIncassoEffettivo)
    {
        var incasso = GetById(id);
        if (incasso == null) return false;

        incasso.Incassato = true;
        incasso.DataIncassoEffettivo = dataIncassoEffettivo;
        return Update(incasso);
    }

    /// <summary>
    /// Calcola il totale degli incassi previsti per un periodo
    /// </summary>
    public decimal GetTotaleIncassiPrevisti(int bancaId, int anno, int mese)
    {
        return _context.BancaIncassi
            .Find(i => i.BancaId == bancaId && i.Anno == anno && i.Mese == mese)
            .Sum(i => i.Importo);
    }

    /// <summary>
    /// Calcola il totale degli incassi effettivi per un periodo
    /// </summary>
    public decimal GetTotaleIncassiEffettivi(int bancaId, int anno, int mese)
    {
        return _context.BancaIncassi
            .Find(i => i.BancaId == bancaId && i.Anno == anno && i.Mese == mese && i.Incassato)
            .Sum(i => i.Importo);
    }
}

