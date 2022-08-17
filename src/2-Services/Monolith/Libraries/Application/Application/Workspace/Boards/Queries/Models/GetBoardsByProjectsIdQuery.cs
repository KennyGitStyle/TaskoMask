﻿using System.Collections.Generic;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Boards;
using TaskoMask.BuildingBlocks.Application.Queries;


namespace TaskoMask.Services.Monolith.Application.Workspace.Boards.Queries.Models
{
   
    public class GetBoardsByProjectsIdQuery : BaseQuery<IEnumerable<BoardBasicInfoDto>>
    {
        public GetBoardsByProjectsIdQuery(string[] projectsId)
        {
            ProjectsId = projectsId;
        }

        public string[] ProjectsId { get; }
    }
}