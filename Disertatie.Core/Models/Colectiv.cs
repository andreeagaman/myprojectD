using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Models
{
   public class Colectiv
    {
        public virtual int Id { get; set; }
        public virtual string Nume { get; set; }

        public virtual IEnumerable<Colectiv> GetColective()
        {
            IEnumerable<Colectiv> colective;
            var colectiveRep = new Repositories.Repository<int, Colectiv>();

            colective = (from colectiv in colectiveRep.QueryAll()
                         orderby colectiv.Nume
                         select colectiv).ToList();

            return colective;
        }

    }
}
