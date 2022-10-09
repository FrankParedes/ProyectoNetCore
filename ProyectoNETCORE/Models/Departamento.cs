using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProyectoNETCORE.Models
{
    public partial class Departamento
    {
        public Departamento()
        {
            Provincia = new HashSet<Provincia>();
            Trabajadores = new HashSet<Trabajadores>();
        }

        public int Id { get; set; }
        [Display(Name = "Departamento")]
        public string? NombreDepartamento { get; set; }

        public virtual ICollection<Provincia> Provincia { get; set; }
        public virtual ICollection<Trabajadores> Trabajadores { get; set; }
    }
}
