using AutoMapper;
using DataAccessLayer.Entities;
using PresentationLayer.Models;

namespace PresentationLayer.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryViewModel, Category>().ReverseMap();
        }
    }
}
