using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.models.Queries.DownloadModel
{
    public class DownloadModelHandler : IRequestHandler<DownloadModelCommand, byte[]>
    {
        public Task<byte[]> Handle(DownloadModelCommand request, CancellationToken cancellationToken)
        {
            byte[] file;
          
            try
            {
                file = File.ReadAllBytes(request.FilePath);
            }
            catch
            {
                return Task.FromResult(Array.Empty<byte>());
            }

            return Task.FromResult(file);

        }
    }
}