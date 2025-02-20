

using AlumniManagement.WCF.Entities;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.WCF
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ModelMapping>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {

            CreateMap<MajorDTO, Major>().ReverseMap();
            CreateMap<HistoryDTO, JobHistory>().ReverseMap();
            CreateMap<AlumniDTO, Alumni>().ReverseMap();
            CreateMap<FacultyDTO, Faculty>().ReverseMap();
            CreateMap<DistrictDTO, District>().ReverseMap();
            CreateMap<StateDTO, State>().ReverseMap();
            CreateMap<JobDTO, JobHistory>().ReverseMap();
            CreateMap<ImageDTO, AlumniImage>().ReverseMap();
            CreateMap<HobbyDTO, Hobby>().ReverseMap();


        }
    }
}
