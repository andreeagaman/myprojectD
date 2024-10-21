using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Disertatie.Models;
using Disertatie.Core.Models;

namespace Disertatie.Areas.Admin.Models
{
    public class UserModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage="Numele trebuie sa aiba cel mult 100 de caractarere!")]
        [Required(ErrorMessage="Numele este obligatoriu!")]
        public string Nume {get; set;}

        [StringLength(100, ErrorMessage="Prenumele trebuie sa aiba cel mult 100 de caractarere")]
        [Required(ErrorMessage="Prenumele este obligatoriu!")]
        public string Prenume {get; set;}

       
        public Colectiv Colectiv {get; set;}

       
        public ICollection <Rol> Rol  {get; set;}


        [Display(Name = "Nume utilizator")]
        [RegularExpression(Regexes.Username, ErrorMessage="Numele de utilizator nu este valid!")]
        [Required( ErrorMessage= "Numele de utilizator este obligatoriu!")]
        public string Username { get; set; }

        [RegularExpression(Regexes.Email,ErrorMessage="E-mailul nu este valid!")]
        [Required(ErrorMessage="E-mailul este obligatoriu!")]
        public string Email { get; set; }
        
        [DataType(DataType.Password, ErrorMessage="Parola nu este valida!")]
        [Display(Name = "Parola")]
        public string Parola { get; set; }

        [DataType(DataType.Password, ErrorMessage="Parola confirmata nu este valida!")]
        [Display(Name = "Confirma parola")]
        [System.Web.Mvc.Compare("Parola",ErrorMessage="Parolele nu corespund!")]
        public string ConfirmareParola { get; set; }

        [Display(Name = "Intrebare secreta")]
        [Required(ErrorMessage = "Acest camp este obligatoriu")]
        public string IntrebareParola { get; set; }

        [Display(Name = "Raspuns la intrebare secreta")]
        [Required(ErrorMessage = "Acest camp este obligatoriu")]
        public string RaspunsParola { get; set; }

        [Display(Name = "Comentarii")]
        [DataType(DataType.MultilineText)]
        public string Comentariu { get; set; }

        [Display(Name = "Este activ?")]
        public bool Aprobat { get; set; }

        public IList<Colectiv> Colective { get; set; }
        public IList<Rol> Roluri { get; set; }

        [StringLength(255, ErrorMessage="Titlul este prea mare!")]
        public string Titlu { get; set; }
    }
}