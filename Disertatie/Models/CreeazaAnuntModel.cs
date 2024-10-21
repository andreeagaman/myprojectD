using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Disertatie.Models
{
    public class CreeazaAnuntModel
    {
        [Required(ErrorMessage = "Titlul este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractere!")]
        public string Titlu { get; set; }
        [Required(ErrorMessage = "Detaliile sunt obligatorii!")]
        [StringLength(500, ErrorMessage = "Detaliile nu pot avea mai mult de 500 de caractere!")]
        public string Detalii { get; set; }
    }
}