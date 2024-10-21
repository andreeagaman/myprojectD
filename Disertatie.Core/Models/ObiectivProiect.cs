using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class ObiectivProiect
    {
        public virtual long Id { get; set; }
        public virtual string Descriere { get; set; }
        public virtual int Tip { get; set; } //1-ob general, 2 -ob specific
        public virtual Proiect Proiect { get; set; }
        public virtual bool Activ { get; set; } 
    }
}
