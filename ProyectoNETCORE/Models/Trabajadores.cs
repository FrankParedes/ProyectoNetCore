using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoNETCORE.Models
{
    public partial class Trabajadores
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Obligatorio")]
        [Display(Name = "Tipo Documento")]
        public string? TipoDocumento { get; set; }

        [Display(Name = "Nro Documento")]
        [Required(ErrorMessage = "Campo Obligatorio")]        
        public string? NumeroDocumento { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        public string? Nombres { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]

        public string? Sexo { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]

        public int? IdDepartamento { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]

        public int? IdProvincia { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]

        public int? IdDistrito { get; set; }

        public virtual Departamento? IdDepartamentoNavigation { get; set; }
        public virtual Distrito? IdDistritoNavigation { get; set; }
        public virtual Provincia? IdProvinciaNavigation { get; set; }
    }
}
