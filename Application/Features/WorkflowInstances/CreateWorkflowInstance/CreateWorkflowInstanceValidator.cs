
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Results;


namespace Application.Features.WorkflowInstances.CreateWorkflowInstance
{
    public class CreateWorkflowInstanceValidator : ICreateWorkflowInstanceValidator
    {
        private readonly IUserRepository _userRepository;

        public CreateWorkflowInstanceValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Validate(CreateWorkflowInstanceCommand request)
        {
            User? user = await _userRepository.GetUserById(request.UserId);
            Result<bool> userValidationResult = ValidateUser(user);

            if (!userValidationResult.IsSuccess)
            {
                return userValidationResult;
            }


            return await Task.FromResult(Result<bool>.Success(true));
        }

        private Result<bool> ValidateUser(User? user)
        {
            if (user is null)
            {
                return Result<bool>.Failure(new Error(404, "Usuário não encontrado"));
            }

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
                return Result<bool>.Failure(new Error(401, "Usuário não autorizado"));
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