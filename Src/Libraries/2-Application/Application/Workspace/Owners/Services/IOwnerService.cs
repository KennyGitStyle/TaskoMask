﻿using TaskoMask.Application.Share.Helpers;
using System.Threading.Tasks;
using TaskoMask.Application.Share.Dtos.Authorization.Users;
using TaskoMask.Application.Share.Dtos.Workspace.Owners;
using TaskoMask.Application.Share.ViewModels;
using TaskoMask.Application.Common.Services;

namespace TaskoMask.Application.Workspace.Owners.Services
{
    public interface IOwnerService : IBaseService
    {
        Task<Result<CommandResult>> CreateAsync(OwnerRegisterDto input);
        Task<Result<CommandResult>> UpdateAsync(OwnerUpdateDto input);
        Task<Result<OwnerBasicInfoDto>> GetByIdAsync(string id);
        Task<Result<PaginatedListReturnType<OwnerOutputDto>>> SearchAsync(int page, int recordsPerPage, string term);
        Task<Result<OwnerDetailsViewModel>> GetDetailsAsync(string id);
    }
}