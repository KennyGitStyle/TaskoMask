﻿using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Owners;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Authorization.Users;
using TaskoMask.Services.Monolith.Application.Core.Queries;


namespace TaskoMask.Services.Monolith.Application.Workspace.Owners.Queries.Models
{
    public class GetOwnerByIdQuery : BaseQuery<OwnerBasicInfoDto>
    {
        public GetOwnerByIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
