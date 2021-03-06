using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models;
using ProjektDyplomowy.Models.Posts;

namespace ProjektDyplomowy.Profiles
{
    public class AutoMapperDefaultProfile : Profile
    {
        public AutoMapperDefaultProfile()
        {
            CreateMap<Category, SelectListItem>()
                .ForMember(des => des.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.Value, opt => opt.MapFrom(src => src.Id));

            CreateMap<Post, PostsIndexViewModel>()
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Post, PostsDetailsViewModel>()
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<PostsAddViewModel, Post>()
                .ForMember(des => des.FileName, opt => opt.Ignore())
                .ForMember(des => des.ContentType, opt => opt.MapFrom(src => src.FileType - 1))
                .ForMember(des => des.SourceType, opt => opt.MapFrom(src => src.FileSource - 1));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<User, string>()
                .ConvertUsing(src => src.Id.ToString());

            CreateMap<Post, PostsEditViewModel>()
                .ForMember(des => des.SelectCategories, opt => opt.Ignore())
                .ReverseMap();
            //.ForMember(src => src.Tags, opt => opt.Ignore());

            CreateMap<Tag, string>()
                .ConvertUsing(src => src.Name.ToString());

            CreateMap<string, Tag>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src));

            CreateMap<Category, CategoryViewModel>().ReverseMap();
        }
    }
}
