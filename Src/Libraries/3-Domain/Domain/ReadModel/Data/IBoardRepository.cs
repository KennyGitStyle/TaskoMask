﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TaskoMask.Domain.Core.Data;
using TaskoMask.Domain.ReadModel.Entities;

namespace TaskoMask.Domain.ReadModel.Data
{

    public interface IBoardRepository : IBaseRepository<Board>
    {
        Task<IEnumerable<Board>> GetListByProjectIdAsync(string projectId);
        Task<IEnumerable<Board>> GetListByOrganizationIdAsync(string organizationId);
        Task<bool> ExistByNameAsync(string id, string name);
        Task<long> CountByProjectIdAsync(string projectId);
        IEnumerable<Board> Search(int page, int recordsPerPage, string term, out int pageSize, out int totalItemCount);
    }
}