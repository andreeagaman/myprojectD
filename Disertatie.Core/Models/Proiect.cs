using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class Proiect
    {
        public virtual long Id { get; set; }
        public virtual DateTime DataCreare { get; set; }
        public virtual DateTime DataModificareStare { get; set; }
        public virtual string Titlu { get; set; }
        public virtual int? Durata { get; set; }//in luni
        public virtual string Detalii { get; set; }
        public virtual Utilizator Initiator { get; set; }
        public virtual int Status { get; set; } //1 Propus 2 In lucru 3 Finalizat 4 Anulat
        public virtual int NumarMembriiProiect { get; set; }
        public virtual int MembriiNecesari { get; set; }
        public virtual int Evaluare { get; set; }
        public virtual int TotalEvaluatori { get; set; }
    }
}
