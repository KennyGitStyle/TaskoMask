﻿using AutoMapper;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using System.Threading.Tasks;
using TaskoMask.Services.Monolith.Application.Workspace.Tasks.Commands.Models;
using TaskoMask.Services.Monolith.Application.Workspace.Tasks.Queries.Models;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Tasks;
using System.Collections.Generic;
using TaskoMask.BuildingBlocks.Application.Notifications;
using TaskoMask.BuildingBlocks.Application.Bus;
using TaskoMask.BuildingBlocks.Contracts.ViewModels;
using TaskoMask.Services.Monolith.Application.Workspace.Cards.Queries.Models;
using TaskoMask.Services.Monolith.Application.Workspace.Activities.Services;
using TaskoMask.Services.Monolith.Application.Workspace.Comments.Services;
using TaskoMask.BuildingBlocks.Contracts.Models;
using TaskoMask.BuildingBlocks.Application.Services;

namespace TaskoMask.Services.Monolith.Application.Workspace.Tasks.Services
{
    public class TaskService : ApplicationService, ITaskService
    {
        #region Fields

        private readonly IActivityService _activityService;
        private readonly ICommentService _commentService;

        #endregion

        #region Ctors

        public TaskService(IInMemoryBus inMemoryBus, IMapper mapper, INotificationHandler notifications, IActivityService activityService, ICommentService commentService) : base(inMemoryBus, mapper, notifications)
        {
            _activityService = activityService;
            _commentService = commentService;
        }


        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> AddAsync(AddTaskDto input)
        {
            var cmd = new AddTaskCommand( title: input.Title, cardId: input.CardId, description: input.Description);
            return await SendCommandAsync(cmd);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> UpdateAsync(UpdateTaskDto input)
        {
            var cmd = new UpdateTaskCommand(id: input.Id, title: input.Title, description: input.Description);
            return await SendCommandAsync(cmd);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> MoveTaskToAnotherCardAsync(string taskId, string cardId)
        {
            var cmd = new MoveTaskToAnotherCardCommand(taskId, cardId);
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
        public async Task<Result<TaskDetailsViewModel>> GetDetailsAsync(string id)
        {

            var taskQueryResult = await GetByIdAsync(id);
            if (!taskQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(taskQueryResult.Errors);


            var cardQueryResult = await SendQueryAsync(new GetCardByIdQuery(taskQueryResult.Value.CardId));
            if (!cardQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(cardQueryResult.Errors);


            var activitiesQueryResult = await _activityService.GetListByTaskIdAsync(id);
            if (!activitiesQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(activitiesQueryResult.Errors);


            var commentsQueryResult = await _commentService.GetListByTaskIdAsync(id);
            if (!commentsQueryResult.IsSuccess)
                return Result.Failure<TaskDetailsViewModel>(commentsQueryResult.Errors);



            var taskDetail = new TaskDetailsViewModel
            {
                Task = taskQueryResult.Value,
                Card = cardQueryResult.Value,
                Activities= activitiesQueryResult.Value,
                Comments= commentsQueryResult.Value,
            };

            return Result.Success(taskDetail);

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
        public async Task<Result<PaginatedList<TaskOutputDto>>> SearchAsync(int page, int recordsPerPage, string term)
        {
            return await SendQueryAsync(new SearchTasksQuery(page, recordsPerPage, term));
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<long>> CountAsync()
        {
            return await SendQueryAsync(new GetTasksCountQuery());
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> DeleteAsync(string id)
        {
            var cmd = new DeleteTaskCommand(id);
            return await SendCommandAsync(cmd);
        }





        #endregion
    }
}
