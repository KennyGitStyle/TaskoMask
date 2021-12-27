﻿using TaskoMask.Application.Share.Dtos.Team.Projects;
using TaskoMask.Application.Share.Helpers;
using TaskoMask.Application.Share.ViewModels;
using TaskoMask.Presentation.Framework.Share.Contracts;
using TaskoMask.Presentation.Framework.Share.Helpers;
using TaskoMask.Presentation.Framework.Share.Services.Http;

namespace TaskoMask.Presentation.UI.UserPanel.Services.Data
{
    public class ProjectClientService : IProjectClientService
    {
        #region Fields

        private readonly IHttpClientService _httpClientService;

        #endregion

        #region Ctor

        public ProjectClientService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<ProjectDetailsViewModel>> Get(string id)
        {
            var uri = new ClientUriBuilder(new Uri(_httpClientService.GetBaseAddress(), $"/projects/{id}")).Uri;

            return await _httpClientService.GetAsync<ProjectDetailsViewModel>(uri);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> Create(ProjectUpsertDto input)
        {
            var uri = new ClientUriBuilder(new Uri(_httpClientService.GetBaseAddress(), $"/projects")).Uri;
            return await _httpClientService.PostAsync<CommandResult>(uri, input);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<Result<CommandResult>> Update(ProjectUpsertDto input)
        {
            var uri = new ClientUriBuilder(new Uri(_httpClientService.GetBaseAddress(), $"/projects")).Uri;
            return await _httpClientService.PutAsync<CommandResult>(uri, input);
        }

        #endregion

        #region Private Methods



        #endregion

    }
}