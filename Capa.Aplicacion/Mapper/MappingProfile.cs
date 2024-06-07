using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Datos.Entidades;


namespace Capa.Aplicacion.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Producto, ProductoDTO>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria))
                .ForPath(dest => dest.Categoria.NombreCategoria, opt => opt.MapFrom(src => src.Categoria.Nombre)).ReverseMap();
            
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
