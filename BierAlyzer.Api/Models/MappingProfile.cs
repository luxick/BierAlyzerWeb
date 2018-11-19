using AutoMapper;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.EntityModel;

namespace BierAlyzer.Api.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Contains the BierAlyzer.Api mapping profiles </summary>
    /// <remarks>   Andre Beging, 17.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class MappingProfile : Profile
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        /// <remarks>   Andre Beging, 17.11.2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public MappingProfile()
        {
            CreateMap<Drink, DrinkDto>();
            CreateMap<DrinkDto, Drink>();

            CreateMap<Event, EventDto>();
            CreateMap<EventDto, Event>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
