﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskoMask.Application.Share.Dtos.Authorization.Users;
using TaskoMask.Application.Workspace.Members.Services;
using TaskoMask.Application.Share.Helpers;
using TaskoMask.Presentation.Framework.Web.Controllers;
using TaskoMask.Presentation.Framework.Share.Services.Authentication.JwtAuthentication;
using TaskoMask.Domain.Share.Models;
using TaskoMask.Application.Share.Dtos.Workspace.Members;
using TaskoMask.Presentation.Framework.Share.Contracts;
using TaskoMask.Application.Authorization.Users.Services;

namespace TaskoMask.Presentation.API.UserPanelAPI.Controllers
{
    public class AccountController : BaseApiController, IAccountClientService
    {
        #region Fields

        private readonly IMemberService _memberService;
        private readonly IUserService _userService;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;

        #endregion

        #region Ctor

        public AccountController(IJwtAuthenticationService jwtAuthenticationService, IMemberService memberService, IMapper mapper, IUserService userService) : base(mapper)
        {
            _jwtAuthenticationService = jwtAuthenticationService;
            _memberService = memberService;
            _userService = userService;
        }


        #endregion

        #region Public Methods



        /// <summary>
        /// login member - return jwt token if login is success
        /// </summary>
        [HttpPost]
        [Route("account/login")]
        public async Task<Result<UserJwtTokenDto>> Login([FromBody] UserLoginDto input)
        {
            //get user
            var userQueryResult = await _userService.GetByUserNameAsync(input.UserName);
            if (!userQueryResult.IsSuccess)
                return Result.Failure<UserJwtTokenDto>(userQueryResult.Errors, userQueryResult.Message);

            //validate user password
            var validateQueryResult = await _userService.IsValidCredentialAsync(input.UserName, input.Password);
            if (!validateQueryResult.IsSuccess || !validateQueryResult.Value)
                return Result.Failure<UserJwtTokenDto>(userQueryResult.Errors, validateQueryResult.Message);


            //get member
            var memberQueryResult = await _memberService.GetByIdAsync(userQueryResult.Value.Id);
            if (!memberQueryResult.IsSuccess)
                return Result.Failure<UserJwtTokenDto>(memberQueryResult.Errors, memberQueryResult.Message);


            //map to jwt claims model
            var user = _mapper.Map<AuthenticatedUser>(memberQueryResult.Value);

            //generate jwt token
            var token = _jwtAuthenticationService.GenerateJwtToken(user);

            return Result.Success(value: new UserJwtTokenDto { JwtToken=token});
        }




        /// <summary>
        /// register new member - return jwt token if register is success
        /// </summary>
        [HttpPost]
        [Route("account/register")]
        public async Task<Result<UserJwtTokenDto>> Register([FromBody] MemberRegisterDto input)
        {
            //create user
            var createCommandResult = await _memberService.CreateAsync(input);
            if (!createCommandResult.IsSuccess)
                return Result.Failure<UserJwtTokenDto>(createCommandResult.Errors, createCommandResult.Message);


            //get user
            var userQueryResult = await _userService.GetByUserNameAsync(input.Email);
            if (!userQueryResult.IsSuccess)
                return Result.Failure<UserJwtTokenDto>(userQueryResult.Errors, userQueryResult.Message);


            //get member
            var memberQueryResult = await _memberService.GetByIdAsync(userQueryResult.Value.Id);
            if (!memberQueryResult.IsSuccess)
                return Result.Failure<UserJwtTokenDto>(memberQueryResult.Errors, memberQueryResult.Message);

            //map to jwt claims model
            var user = _mapper.Map<AuthenticatedUser>(memberQueryResult.Value);

            //generate jwt token
            var token = _jwtAuthenticationService.GenerateJwtToken(user);

            return Result.Success(value: new UserJwtTokenDto { JwtToken = token });
        }




        #endregion

        #region  Private Methods






        #endregion

    }


}