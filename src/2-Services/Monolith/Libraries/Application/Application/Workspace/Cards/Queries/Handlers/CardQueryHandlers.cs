﻿using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskoMask.Services.Monolith.Application.Workspace.Cards.Queries.Models;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Cards;
using TaskoMask.BuildingBlocks.Application.Exceptions;
using TaskoMask.BuildingBlocks.Application.Queries;
using TaskoMask.BuildingBlocks.Contracts.Resources;
using TaskoMask.BuildingBlocks.Application.Notifications;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.Services.Monolith.Domain.DataModel.Data;
using TaskoMask.BuildingBlocks.Domain.Resources;
using TaskoMask.BuildingBlocks.Contracts.Models;

namespace TaskoMask.Services.Monolith.Application.Workspace.Cards.Queries.Handlers
{
    public class CardQueryHandlers : BaseQueryHandler,
        IRequestHandler<GetCardByIdQuery, CardBasicInfoDto>,
         IRequestHandler<GetCardsByBoardIdQuery, IEnumerable<CardBasicInfoDto>>,
        IRequestHandler<SearchCardsQuery, PaginatedList<CardOutputDto>>,
        IRequestHandler<GetCardsCountQuery, long>


    {
        #region Fields

        private readonly ICardRepository _cardRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly ITaskRepository _taskRepository;


        #endregion

        #region Ctors

        public CardQueryHandlers(ICardRepository cardRepository, INotificationHandler notifications, IMapper mapper, ITaskRepository taskRepository, IBoardRepository boardRepository) : base(mapper, notifications)
        {
            _cardRepository = cardRepository;
            _taskRepository = taskRepository;
            _boardRepository = boardRepository;
        }

        #endregion

        #region Handlers



        /// <summary>
        /// 
        /// </summary>
        public async Task<CardBasicInfoDto> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByIdAsync(request.Id);
            if (card == null)
                throw new ApplicationException(ContractsMessages.Data_Not_exist, DomainMetadata.Card);

            return _mapper.Map<CardBasicInfoDto>(card);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<IEnumerable<CardBasicInfoDto>> Handle(GetCardsByBoardIdQuery request, CancellationToken cancellationToken)
        {
            var cards = await _cardRepository.GetListByBoardIdAsync(request.BoardId);
            return _mapper.Map<IEnumerable<CardBasicInfoDto>>(cards);
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<PaginatedList<CardOutputDto>> Handle(SearchCardsQuery request, CancellationToken cancellationToken)
        {
            var cards = _cardRepository.Search(request.Page, request.RecordsPerPage, request.Term, out var pageNumber, out var totalCount);
            var cardsDto = _mapper.Map<IEnumerable<CardOutputDto>>(cards);

            foreach (var item in cardsDto)
            {
                var board = await _boardRepository.GetByIdAsync(item.BoardId);
                item.BoardName = board?.Name;
                item.TasksCount = await _taskRepository.CountByCardIdAsync(item.Id);
            }

            return new PaginatedList<CardOutputDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                Items = cardsDto
            };
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<long> Handle(GetCardsCountQuery request, CancellationToken cancellationToken)
        {
            return await _cardRepository.CountAsync();
        }




        #endregion
    }
}