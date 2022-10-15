﻿using AutoMapper;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using System.Collections.Generic;
using TaskoMask.BuildingBlocks.Application.Notifications;
using TaskoMask.BuildingBlocks.Application.Bus;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Activities;
using System.Threading.Tasks;
using TaskoMask.Services.Monolith.Application.Workspace.Activities.Queries.Models;
using TaskoMask.BuildingBlocks.Application.Services;

namespace TaskoMask.Services.Monolith.Application.Workspace.Activities.Services
{
    public class ActivityService : ApplicationService, IActivityService
    {
        #region Fields


        #endregion

        #region Ctors

        public ActivityService(IInMemoryBus inMemoryBus, IMapper mapper, INotificationHandler notifications) : base(inMemoryBus, mapper, notifications)
        { }


        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<IEnumerable<ActivityBasicInfoDto>>> GetListByTaskIdAsync(string taskId)
        {
            return await SendQueryAsync(new GetActivitiesByTaskIdQuery(taskId));
        }




        #endregion
    }
}
