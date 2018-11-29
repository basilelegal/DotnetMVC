//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SitePartage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Leasing
    {
        public int LeasingID { get; set; }


        public Nullable<int> ProductID { get; set; }


        public Nullable<int> UserID { get; set; }

        [Display(Name = "Date")]
        public Nullable<System.DateTime> Date { get; set; }

        [Display(Name = "Durée")]
        public Nullable<int> Duration { get; set; }

        [Display(Name = "Cout total")]
        public Nullable<int> TotalCost { get; set; }

        [Display(Name = "Etat")]
        public string Status { get; set; }

        [Display(Name = "Commentaire déposé")]
        public Nullable<bool> HasComment { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Produit")]
        public virtual Product Product { get; set; }

        [Display(Name = "Membre")]
        public virtual User User { get; set; }
    }
}
