using Application.DTOs.Notebook;
using Application.DTOs.Notes;
using Application.DTOs.User;
using Application.Identity;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, UserAuth>();
            CreateMap<UserRegistrationDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Notebook, NotebookDto>();

            CreateMap<CreateNotebookDto, Notebook>();

            CreateMap<UpdateNotebookDto, Notebook>();

            CreateMap<Note, NoteDto>();

            CreateMap<CreateNoteDto, Note>();
        }
    }
}
