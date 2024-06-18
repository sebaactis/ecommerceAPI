using System.ComponentModel.DataAnnotations;

namespace Capa.Datos.Entidades
{
    public class Categoria : ModelBase
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre de la categoria debe tener minimo 3 caracteres y maximo 20 caracteres.")]
        public string Nombre { get; set; }
        public ICollection<Producto> Productos { get; set; }

    }
}
