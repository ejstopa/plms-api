using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.models.Queries.DownloadModel
{
    public class DownloadModelCommand : IRequest<byte []>
    {
        public string FilePath {get; set;} = string.Empty;
    }
}