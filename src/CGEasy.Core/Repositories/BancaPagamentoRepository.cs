using CGEasy.Core.Data;
using CGEasy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGEasy.Core.Repositories;

/// <summary>
/// Repository per gestione CRUD dei Pagamenti a Fornitori per Banca
/// </summary>
public class BancaPagamentoRepository
{
    private readonly LiteDbContext _context;

    public BancaPagamentoRepository(LiteDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera tutti i pagamenti
    /// </summary>
    public List<BancaPagamento> GetAll()
    {
        return _context.BancaPagamenti.FindAll().ToList();
    }

    /// <summary>
    /// Recupera un pagamento per ID
    /// </summary>
    public BancaPagamento? GetById(int id)
    {
        return _context.BancaPagamenti.FindById(id);
    }

    /// <summary>
    /// Recupera tutti i pagamenti di una banca
    /// </summary>
    public List<BancaPagamento> GetByBancaId(int bancaId)
    {
        return _context.BancaPagamenti
            .Find(p => p.BancaId == bancaId)
            .OrderBy(p => p.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Recupera i pagamenti per anno e mese
    /// </summary>
    public List<BancaPagamento> GetByPeriodo(int bancaId, int anno, int mese)
    {
        return _context.BancaPagamenti
            .Find(p => p.BancaId == bancaId && p.Anno == anno && p.Mese == mese)
            .OrderBy(p => p.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Recupera i pagamenti in scadenza entro una certa data
    /// </summary>
    public List<BancaPagamento> GetInScadenzaEntro(int bancaId, DateTime dataLimite)
    {
        return _context.BancaPagamenti
            .Find(p => p.BancaId == bancaId && !p.Pagato && p.DataScadenza <= dataLimite)
            .OrderBy(p => p.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Recupera i pagamenti non ancora pagati
    /// </summary>
    public List<BancaPagamento> GetNonPagati(int bancaId)
    {
        return _context.BancaPagamenti
            .Find(p => p.BancaId == bancaId && !p.Pagato)
            .OrderBy(p => p.DataScadenza)
            .ToList();
    }

    /// <summary>
    /// Inserisce un nuovo pagamento
    /// </summary>
    public int Insert(BancaPagamento pagamento)
    {
        pagamento.DataCreazione = DateTime.Now;
        pagamento.DataUltimaModifica = DateTime.Now;
        return _context.BancaPagamenti.Insert(pagamento);
    }

    /// <summary>
    /// Aggiorna un pagamento esistente
    /// </summary>
    public bool Update(BancaPagamento pagamento)
    {
        pagamento.DataUltimaModifica = DateTime.Now;
        return _context.BancaPagamenti.Update(pagamento);
    }

    /// <summary>
    /// Elimina un pagamento
    /// </summary>
    public bool Delete(int id)
    {
        return _context.BancaPagamenti.Delete(id);
    }

    /// <summary>
    /// Elimina tutti i pagamenti di una banca
    /// </summary>
    public int DeleteByBancaId(int bancaId)
    {
        return _context.BancaPagamenti.DeleteMany(p => p.BancaId == bancaId);
    }

    /// <summary>
    /// Segna un pagamento come pagato
    /// </summary>
    public bool SegnaPagato(int id, DateTime dataPagamentoEffettivo)
    {
        var pagamento = GetById(id);
        if (pagamento == null) return false;

        pagamento.Pagato = true;
        pagamento.DataPagamentoEffettivo = dataPagamentoEffettivo;
        return Update(pagamento);
    }

    /// <summary>
    /// Calcola il totale dei pagamenti previsti per un periodo
    /// </summary>
    public decimal GetTotalePagamentiPrevisti(int bancaId, int anno, int mese)
    {
        return _context.BancaPagamenti
            .Find(p => p.BancaId == bancaId && p.Anno == anno && p.Mese == mese)
            .Sum(p => p.Importo);
    }

    /// <summary>
    /// Calcola il totale dei pagamenti effettivi per un periodo
    /// </summary>
    public decimal GetTotalePagamentiEffettivi(int bancaId, int anno, int mese)
    {
        return _context.BancaPagamenti
            .Find(p => p.BancaId == bancaId && p.Anno == anno && p.Mese == mese && p.Pagato)
            .Sum(p => p.Importo);
    }
}

