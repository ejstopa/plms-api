using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;
using MediatR;

namespace Application.Features.ItemReservations.Commands.CreateItemNameReservation
{
    public class CreateItemNameReservationHandler : IRequestHandler<CreateItemNameReservationCommand, Result<ItemNameReservationResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IItemNameReservationRepository _itemNameReservationRepository;
        private readonly IItemFamilyRepository _itemFamilyRepository;

        public CreateItemNameReservationHandler(
            IMapper mapper,
            IItemNameReservationRepository itemNameReservationRepository,
            IItemFamilyRepository itemFamilyRepository)
        {
            _mapper = mapper;
            _itemFamilyRepository = itemFamilyRepository;
            _itemNameReservationRepository = itemNameReservationRepository;
        }

        public async Task<Result<ItemNameReservationResponseDto>> Handle(CreateItemNameReservationCommand request, CancellationToken cancellationToken)
        {
            ItemFamily? itemFamily = await _itemFamilyRepository.GetItemFamilyByName(request.ItemFamily);

            if (itemFamily is null)
            {
                return Result<ItemNameReservationResponseDto>.Failure(new Error(404, "Família não encontrada"));
            }

            ItemNameReservation? itemNameReservation = await _itemNameReservationRepository.CreateItemNameReservation(request.ItemFamily, request.UserId);

            if (itemNameReservation is null)
            {
                return Result<ItemNameReservationResponseDto>.Failure(new Error(409, "Ocorreu um erro ao tentar criar o código"));
            }

            return Result<ItemNameReservationResponseDto>.Success(
                _mapper.Map<ItemNameReservationResponseDto>(itemNameReservation));
        }
    }
}