using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class CereriProiect
    {
        public virtual long Id { get; set; }
        public virtual Proiect Proiect { get; set; }
        public virtual Utilizator Destinatar { get; set; }
        public virtual Utilizator Expeditor { get; set; }
        public virtual int Status { get; set; }//1 - in asteptare 2- acceptat 3- respins
        public virtual DateTime DataCreare { get; set; }
        public virtual bool ActivaExpeditor { get; set; }
        public virtual bool ActivaDestinatar { get; set; }
        public virtual DateTime? DataAcceptarii { get; set; }
        public virtual DateTime? DataRespingerii { get; set; }
        public virtual DateTime? DataInactivariiExpeditor { get; set; }
        public virtual DateTime? DataInactivariiDestinatar { get; set; }
    }
}
