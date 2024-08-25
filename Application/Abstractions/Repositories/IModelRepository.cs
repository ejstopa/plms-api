using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IModelRepository
    {
        public Task<Model> CreateModel(Model model);

        public Task<IEnumerable<Model>> GetUserCheckedoutModels(int userId);
    }
}