using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Results;
using MediatR;

namespace Application.Features.Items.Commands.CreateItemAttributeValue
{
    public class CreateItemAttributeValueCommand : IRequest<Result<ItemAttributeValue?>>
    {
        public int ItemId {get; set;}
        public int AttributeId {get; set;}
        public string Value {get; set;} = string.Empty;
    }
}