﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Boiler.Models;
using Boiler.Repositories;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;

namespace Boiler.Services
{
    [Authenticate]
    [RequiredRole("Admin")]
    public class UserService : Service
    {
        public IUserAuthRepository userauth_repository { get; set; }
        public IUserRepository user_repository { get; set; }

        public object Get(GetUsersRequest request) {
            var data = user_repository.All();
            var response = data.Select(x => x.ConvertTo<UserResponse>()).ToList();

            response.ForEach(x => x.IsAdmin = userauth_repository.GetUserAuthByUserName(x.UserName).Roles.Contains(RoleNames.Admin));

            return response;
        }

        public object Get(GetUserRequest request) {
            var data = user_repository.GetById(request.Id);
            var response = data.ConvertTo<UserResponse>();
            response.IsAdmin = userauth_repository.GetUserAuthByUserName(response.UserName).Roles.Contains(RoleNames.Admin);

            return response;
        }

        public object Post(CreateUserRequest request) {
            var data = request.ConvertTo<UserAuth>();
            user_repository.Insert(data, "password");

            return new HttpResult { StatusCode = HttpStatusCode.Created };
        }

        public object Put(UpdateUserRequest request) {
            var data = user_repository.GetById(request.Id);

            var object_to_update = request.ConvertTo<UserAuth>();

            // cannot update username
            object_to_update.UserName = data.UserName;
            user_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetUserRequest { Id = request.Id });
        }

        public object Delete(DeleteUserRequest request) {
            user_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }

        public object Put(UpdateUserPasswordRequest request) {
            user_repository.Update(request.ConvertTo<UserAuth>(), request.Password);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/user/changepassword", "PUT")]
    public class UpdateUserPasswordRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    [Route("/user", "GET")]
    public class GetUsersRequest : IReturn<UserResponse>
    {

    }

    [Route("/user", "GET")]
    [Route("/user/{id}", "GET")]
    public class GetUserRequest : IReturn<List<UserResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/user", "POST")]
    public class CreateUserRequest : IReturn<UserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

    [Route("/user", "PUT")]
    [Route("/user/{id}", "PUT")]
    public class UpdateUserRequest : IReturn<UserResponse>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }

    [Route("/user", "DELETE")]
    [Route("/user/{id}", "DELETE")]
    public class DeleteUserRequest
    {
        public int Id { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequest>
    {
        public UpdateUserPasswordRequestValidator() {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }

    public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
    {
        public GetUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            //RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}