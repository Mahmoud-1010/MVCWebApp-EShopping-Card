using AutoMapper;
using DataAccessLayer.Entities;
using PresentationLayer.Models;

namespace PresentationLayer.Mapper
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel,Product>().ReverseMap();
        }
    }
}
