using AutoMapper;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Creating Game
            CreateMap<CreateGameDto, Game>();
            

            //Update Mapping For Game Entity
            CreateMap<UpdateGameDto, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            //Viewing Game
            CreateMap<Game, GameSummaryDto>();


            CreateMap<Game, GameDetailsDto>();
            

            //Viewing Genre
            CreateMap<Genre, GenreDto>();
            

            //Creating Genre
            CreateMap<GenreCreateDto, Genre>();

            //Update Mapping For Genre Entity
            CreateMap<UpdateGenreDto, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
