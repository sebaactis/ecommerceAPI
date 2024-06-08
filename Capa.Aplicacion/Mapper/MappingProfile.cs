using AutoMapper;
using Capa.Aplicacion.DTI;
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

            CreateMap<Producto, ProductoDTI>().ReverseMap();

            CreateMap<Categoria, CategoriaDTO>().ForMember(dest => dest.NombreCategoria, opt => opt.MapFrom(src => src.Nombre)).ReverseMap();
            CreateMap<Categoria, CategoriaDTI>().ForMember(dest => dest.NombreCategoria, opt => opt.MapFrom(src => src.Nombre)).ReverseMap();
        }
    }
}
