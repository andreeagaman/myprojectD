using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class Anunt
    {
        public virtual long Id { get; set; }
        public virtual DateTime DataCreare { get; set; }
        public virtual string Nume { get; set; }
        public virtual string Detalii { get; set; }
        public virtual Utilizator Utilizator { get; set; }
        public virtual bool Activ { get; set; }
    }
}
