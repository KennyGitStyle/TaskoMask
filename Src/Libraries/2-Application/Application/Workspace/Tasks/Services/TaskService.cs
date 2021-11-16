﻿using AutoMapper;
using TaskoMask.Application.Core.Helpers;
using System.Threading.Tasks;
using TaskoMask.Application.Workspace.Tasks.Commands.Models;
using TaskoMask.Application.Workspace.Tasks.Queries.Models;
using TaskoMask.Application.Core.Dtos.Workspace.Tasks;
using TaskoMask.Application.Core.Commands;
using System.Collections.Generic;
using TaskoMask.Application.Core.Notifications;
using TaskoMask.Application.Core.Bus;
using TaskoMask.Application.Common.Base.Services;
using TaskoMask.Application.Core.ViewModels;
using TaskoMask.Application.Workspace.Cards.Queries.Models;
using TaskoMask.Application.Queries.Models.Boards;
using TaskoMask.Application.Team.Projects.Queries.Models;
using TaskoMask.Application.Team.Organizations.Queries.Models;

namespace TaskoMask.Application.Workspace.Tasks.Services
{
    public class TaskService : BaseService<Domain.Workspace.Entities.Task>, ITaskService
    {
        #region Fields


        #endregion

        #region Ctors

        public TaskService(IInMemoryBus inMemoryBus, IMapper mapper, IDomainNotificationHandler notifications) : base(inMemoryBus, mapper, notifications)
        { }


        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> CreateAsync(TaskUpsertDto input)
        {
            var cmd = new CreateTaskCommand(cardId: input.CardId, title: input.Title, description: input.Description);
            return await SendCommandAsync(cmd);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> UpdateAsync(TaskUpsertDto input)
        {
            var cmd = new UpdateTaskCommand(id: input.Id, name: input.Title, description: input.Description);
            return await SendCommandAsync(cmd);
        }



 


        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<TaskBasicInfoDto>> GetByIdAsync(string id)
        {
            return await SendQueryAsync(new GetTaskByIdQuery(id));
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<IEnumerable<TaskBasicInfoDto>>> GetListByCardIdAsync(string cardId)
        {
            return await SendQueryAsync(new GetTasksByCardIdQuery(cardId));
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<PublicPaginatedListReturnType<TaskOutputDto>>> SearchAsync(int page, int recordsPerPage, string term)
        {
            return await SendQueryAsync(new SearchTasksQuery(page, recordsPerPage, term));
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<TaskDetailsViewModel>> GetDetailsAsync(string id)
        {
            var taskQueryResult = await SendQueryAsync(new GetTaskByIdQuery(id));
            if (!taskQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(taskQueryResult.Errors);



            var cardQueryResult = await SendQueryAsync(new GetCardByIdQuery(taskQueryResult.Value.CardId));
            if (!cardQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(cardQueryResult.Errors);


            var boardQueryResult = await SendQueryAsync(new GetBoardByIdQuery(cardQueryResult.Value.BoardId));
            if (!boardQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(boardQueryResult.Errors);


            var projectQueryResult = await SendQueryAsync(new GetProjectByIdQuery(boardQueryResult.Value.ProjectId));
            if (!projectQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(projectQueryResult.Errors);


            var organizationQueryResult = await SendQueryAsync(new GetOrganizationByIdQuery(projectQueryResult.Value.OrganizationId));
            if (!organizationQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(organizationQueryResult.Errors);


            var cardReportQueryResult = await SendQueryAsync(new GetCardReportQuery(id));
            if (!cardReportQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(cardReportQueryResult.Errors);


          
            var cardDetail = new TaskDetailsViewModel
            {
                Organization = organizationQueryResult.Value,
                Project = projectQueryResult.Value,
                Board = boardQueryResult.Value,
                Reports = cardReportQueryResult.Value,
                Card = cardQueryResult.Value,
                Task = taskQueryResult.Value,
            };

            return Result.Success(cardDetail);
        }





        #endregion
    }
}