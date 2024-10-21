using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class ComentariuProiect
    {
        public virtual long Id { get; set; }
        public virtual Proiect Proiect { get; set; }
        public virtual Utilizator Utilizator { get; set; }
        public virtual DateTime DataCrearii { get; set; }
        public virtual string Descriere { get; set; }
        public virtual bool Activ { get; set; }
    }
}
