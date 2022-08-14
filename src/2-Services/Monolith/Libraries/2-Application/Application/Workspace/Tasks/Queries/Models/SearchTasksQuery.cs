﻿using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Tasks;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.Services.Monolith.Application.Core.Queries;
using TaskoMask.BuildingBlocks.Contracts.Models;

namespace TaskoMask.Services.Monolith.Application.Workspace.Tasks.Queries.Models
{
    public class SearchTasksQuery : BaseQuery<PaginatedList<TaskOutputDto>>
    {
        public SearchTasksQuery(int page, int recordsPerPage, string term)
        {
            Page = page;
            RecordsPerPage = recordsPerPage;
            Term = term;
        }

        public int Page { get; }
        public int RecordsPerPage { get; }
        public string Term { get; }
    }
}
