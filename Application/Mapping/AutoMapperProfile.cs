using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.ItemFamilies;
using Application.Features.ItemReservations.Commands;
using Application.Features.Items;
using Application.Features.Library;
using Application.Features.models;
using Application.Features.Users;
using Application.Features.WorkflowInstances;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Model, ModelResponseDto>();
            CreateMap<UserFile, UserWorkspaceFileResponse>();
            CreateMap<LibraryDirectory, LibraryDirectoryResponse>();
            CreateMap<Item, ItemResponseDto>();
            CreateMap<ItemNameReservation, ItemNameReservationResponseDto>();
            CreateMap<ItemFamily, ItemFamilyResponseDto>();
            CreateMap<WorkflowInstance, WorkflowInstanceResponseDto>();
        }
    }
}