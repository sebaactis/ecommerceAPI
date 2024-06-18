using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.DTO
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre de la categoria debe tener minimo 3 caracteres y maximo 20 caracteres.")]
        public string NombreCategoria { get; set; }

    }
}
