using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Disertatie.Core.Models;

namespace Disertatie.Models
{
    public class MembriiDepartamentHomeModel
    {
        public IList<Utilizator> MembriiDepartament { get; set; }
        public Utilizator utilizatorCurent { get; set; }

    }
}
