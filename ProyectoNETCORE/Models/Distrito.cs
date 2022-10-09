using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProyectoNETCORE.Models
{
    public partial class Distrito
    {
        public Distrito()
        {
            Trabajadores = new HashSet<Trabajadores>();
        }

        public int Id { get; set; }
        public int? IdProvincia { get; set; }
        [Display(Name = "Distrito")]
        public string? NombreDistrito { get; set; }

        public virtual Provincia? IdProvinciaNavigation { get; set; }
        public virtual ICollection<Trabajadores> Trabajadores { get; set; }
    }
}
