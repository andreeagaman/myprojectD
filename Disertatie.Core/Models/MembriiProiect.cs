using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class MembriiProiect
    {
        public virtual long Id { get; set; }
        public virtual Utilizator Utilizator { get; set; }
        public virtual Proiect Proiect { get; set; }
        public virtual bool Activ { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as MembriiProiect;
            if (t == null)
                return false;
            if (Proiect == t.Proiect && Utilizator == t.Utilizator)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (Proiect.Id + "|" + Utilizator.Id).GetHashCode();
        }  
    }
}
