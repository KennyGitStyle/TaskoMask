﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskoMask.BuildingBlocks.Contracts.Resources;
using TaskoMask.BuildingBlocks.Application.Commands;
using TaskoMask.BuildingBlocks.Application.Exceptions;
using TaskoMask.Services.Monolith.Application.Workspace.Owners.Commands.Models;
using TaskoMask.BuildingBlocks.Application.Bus;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.Services.Monolith.Domain.DomainModel.Workspace.Owners.Data;
using TaskoMask.Services.Monolith.Domain.DomainModel.Workspace.Owners.Entities;
using TaskoMask.Services.Monolith.Domain.DomainModel.Workspace.Owners.ValueObjects.Owners;
using TaskoMask.BuildingBlocks.Domain.Resources;

namespace TaskoMask.Services.Monolith.Application.Workspace.Owners.Commands.Handlers
{
    public class OwnerCommandHandlers : BaseCommandHandler,
        IRequestHandler<RegisterOwnerCommand, CommandResult>,
        IRequestHandler<UpdateOwnerProfileCommand, CommandResult>

    {
        #region Fields

        private readonly IOwnerAggregateRepository _ownerAggregateRepository;

        #endregion

        #region Ctors


        public OwnerCommandHandlers(IOwnerAggregateRepository ownerAggregateRepository, IInMemoryBus inMemoryBus) : base(inMemoryBus)
        {
            _ownerAggregateRepository = ownerAggregateRepository;
        }

        #endregion

        #region Handlers



        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = Owner.RegisterOwner(request.Id,request.DisplayName, request.Email);

            await _ownerAggregateRepository.CreateAsync(owner);
            await PublishDomainEventsAsync(owner.DomainEvents);

            return new CommandResult(ContractsMessages.Create_Success, owner.Id.ToString());
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(UpdateOwnerProfileCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerAggregateRepository.GetByIdAsync(request.Id);
            if (owner == null)
                throw new ApplicationException(ContractsMessages.Data_Not_exist, DomainMetadata.Owner);

            var loadedVersion = owner.Version;

            owner.UpdateOwnerProfile(
                OwnerDisplayName.Create(request.DisplayName),
                OwnerEmail.Create(request.Email));

            await _ownerAggregateRepository.ConcurrencySafeUpdate(owner, loadedVersion);

            await PublishDomainEventsAsync(owner.DomainEvents);

            return new CommandResult(ContractsMessages.Update_Success, owner.Id.ToString());
        }



        #endregion

    }
}