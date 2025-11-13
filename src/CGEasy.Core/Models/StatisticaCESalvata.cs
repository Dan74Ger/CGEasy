using LiteDB;
using System;

namespace CGEasy.Core.Models
{
    public class StatisticaCESalvata
    {
        [BsonId]
        public int Id { get; set; }
        public string NomeStatistica { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public int TemplateMese { get; set; }
        public int TemplateAnno { get; set; }
        public string TemplateDescrizione { get; set; } = string.Empty;
        public string PeriodiJson { get; set; } = string.Empty; // Lista di (Mese, Anno) selezionati
        public string DatiStatisticheJson { get; set; } = string.Empty; // Dati serializzati di BilancioStatisticaMultiPeriodo
        public DateTime DataCreazione { get; set; }
        public int UtenteId { get; set; }
        public string NomeUtente { get; set; } = string.Empty;
    }
}

