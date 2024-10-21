using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disertatie.Models
{
    public class EvaluareProiect
    {
        public long IdProiect { get; set; }
        public int Evaluare { get; set; }
        public int TotalEvaluatori { get; set; }
        public double EvaluareMedie { get; set; }

    }
}