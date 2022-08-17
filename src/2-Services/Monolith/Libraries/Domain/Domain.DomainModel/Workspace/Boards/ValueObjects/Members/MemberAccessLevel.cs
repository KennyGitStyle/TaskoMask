﻿using System;
using System.Collections.Generic;
using TaskoMask.BuildingBlocks.Domain.Exceptions;
using TaskoMask.BuildingBlocks.Domain.Models;
using TaskoMask.BuildingBlocks.Contracts.Enums;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.BuildingBlocks.Contracts.Resources;

namespace TaskoMask.Services.Monolith.Domain.DomainModel.Workspace.Boards.ValueObjects.Members
{
    public class MemberAccessLevel : BaseValueObject
    {
        #region Properties

        public BoardMemberAccessLevel Value { get; private set; }


        #endregion

        #region Ctors

        public MemberAccessLevel(BoardMemberAccessLevel value)
        {
            Value = value;

            CheckPolicies();
        }

        #endregion

        #region  Methods



        /// <summary>
        /// Factory method for creating new object
        /// </summary>
        public static MemberAccessLevel Create(BoardMemberAccessLevel value)
        {
            return new MemberAccessLevel(value);
        }



        /// <summary>
        /// 
        /// </summary>
        protected override void CheckPolicies()
        {
           
        }



        /// <summary>
        /// 
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }


        #endregion

    }
}