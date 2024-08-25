using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Library;
using Application.Features.models;
using Application.Features.models.Commands;
using Application.Features.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateModelCommand, Model>();
            CreateMap<Model, ModelResponseDto>();
            CreateMap<UserFile, UserWorkspaceFileResponse>();
            CreateMap<LibraryDirectory, LibraryDirectoryResponse>();
        }
    }
}