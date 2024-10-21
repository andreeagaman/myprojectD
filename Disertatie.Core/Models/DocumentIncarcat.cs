using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class DocumentIncarcat
    {
        public virtual long Id { get; set; }
        public virtual Proiect Proiect { get; set; }
        public virtual string Nume { get; set; }
        public virtual string Cale { get; set; }
        public virtual Utilizator Utilizator { get; set; }
        public virtual DateTime DataIncarcarii { get; set; }
        public virtual bool Activ { get; set; }
    }
}
