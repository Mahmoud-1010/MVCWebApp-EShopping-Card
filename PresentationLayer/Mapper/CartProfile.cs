using AutoMapper;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PresentationLayer.Models;

namespace PresentationLayer.Mapper
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
            CreateMap<ShoppingCart,ShoppingItemViewModel>().ReverseMap();
        }
    }
}
