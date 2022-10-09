using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProyectoNETCORE.Models
{
    public partial class Provincia
    {
        public Provincia()
        {
            Distritos = new HashSet<Distrito>();
            Trabajadores = new HashSet<Trabajadores>();
        }

        public int Id { get; set; }
        public int? IdDepartamento { get; set; }
        [Display(Name = "Provincia")]
        public string? NombreProvincia { get; set; }

        public virtual Departamento? IdDepartamentoNavigation { get; set; }
        public virtual ICollection<Distrito> Distritos { get; set; }
        public virtual ICollection<Trabajadores> Trabajadores { get; set; }
    }
}
