using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
    public class Mesaj
    {
        public virtual long Id { get; set; }
        public virtual DateTime DataTrimitere { get; set; }
        public virtual string Titlu { get; set; }
        public virtual string Detalii { get; set; }
        public virtual Utilizator Destinatar { get; set; }
        public virtual Utilizator Expeditor { get; set; }
        public virtual bool Citit
        {
            get;
            set;
        }
        public virtual bool Activ { get; set; }
    }
}
