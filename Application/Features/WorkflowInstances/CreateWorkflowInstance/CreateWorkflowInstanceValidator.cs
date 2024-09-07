
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;


namespace Application.Features.WorkflowInstances.CreateWorkflowInstance
{
    public class CreateWorkflowInstanceValidator : ICreateWorkflowInstanceValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemNameReservationRepository _itemNameReservationRepository;

        public CreateWorkflowInstanceValidator(IUserRepository userRepository, IItemRepository itemRepository, IItemNameReservationRepository itemNameReservationRepository)
        {
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _itemNameReservationRepository = itemNameReservationRepository;
        }

        public async Task<Result<bool>> Validate(CreateWorkflowInstanceCommand request)
        {
            Result<bool> userValidationResult = await ValidateUser(request.UserId);

            if (!userValidationResult.IsSuccess)
            {
                return userValidationResult;
            }

            Result<bool> ItemValidationResult = await ValidateItem(request);

            if (!ItemValidationResult.IsSuccess)
            {
                return ItemValidationResult;
            }

            return await Task.FromResult(Result<bool>.Success(true));
        }

        private async Task<Result<bool>> ValidateUser(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);

            if (user is null)
            {
                return Result<bool>.Failure(new Error(404, "Usuário não encontrado"));
            }

            if (!user.IsActive)
            {
                return Result<bool>.Failure(new Error(404, "Usuário inativo"));
            }

            if (!ValidateUserRole(user))
            {
                return Result<bool>.Failure(new Error(401, "Usuário não autorizado para inciar fluxos de liberação"));
            }

            return Result<bool>.Success(true);
        }

        private async Task<Result<bool>> ValidateItem(CreateWorkflowInstanceCommand request)
        {
            Item? item = await _itemRepository.GetItemByName(request.ItemName);

            if (item != null && item.CheckedOutBy == 0)
            {
                return Result<bool>.Failure(new Error(400, "O item não está reservado"));
            }

            if (item != null && item.CheckedOutBy != request.UserId)
            {
                return Result<bool>.Failure(new Error(401, "O item está reservado para outro usuário"));
            }

            if (item == null)
            {
                ItemNameReservation? itemNameReservation = await _itemNameReservationRepository.GetItemNameReservationByName(request.ItemName);

                if (itemNameReservation is null)
                {
                    return Result<bool>.Failure(new Error(404, "O item não tem código reservado"));
                }

                if (itemNameReservation.UserId != request.UserId)
                {
                    return Result<bool>.Failure(new Error(401, "O código desse item foi reservado por outro usuário"));
                }

            }

            return Result<bool>.Success(true);
        }

        private bool ValidateUserRole(User user)
        {
            if (user.RoleId != 1 && user.RoleId != 6)
            {
                return false;
            }

            return true;
        }

    }
}