﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Boiler.Models;
using Boiler.Repositories;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Boiler.Services
{
    public class PlayerService : SecureBaseService
    {
        public IPlayerRepository player_repository { get; set; }

        public object Get(GetPlayersRequest request) {
            
            var data = player_repository.All();
            var response = data.Select(x => x.ConvertTo<PlayerResponse>()).ToList();

            return response;
        }

        public object Get(GetPlayerRequest request) {
            var data = player_repository.GetById(request.Id);
            var response = data.ConvertTo<PlayerResponse>();

            return response;
        }

        public object Post(CreatePlayerRequest request) {
            var data = request.ConvertTo<Player>();
            var id = player_repository.Insert(data);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return Get(new GetPlayerRequest {Id = id});
        }

        public object Put(UpdatePlayerRequest request) {
            var object_to_update = request.ConvertTo<Player>();
            player_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetPlayerRequest {Id = request.Id});
        }

        public object Delete(DeletePlayerRequest request) {
            player_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/player", "GET")]
    public  class GetPlayersRequest : IReturn<PlayerResponse>
    {
        
    }

    [Route("/player", "GET")]
    [Route("/player/{id}", "GET")]
    public class GetPlayerRequest : IReturn<List<PlayerResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/player", "POST")]
    public class CreatePlayerRequest : IReturn<PlayerResponse>
    {
        public string PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        public int LeagueAge { get; set; }
        public string LeaguePlayed { get; set; }
        public string ASMI { get; set; }
        public int? HBB { get; set; }
        public int? HSO { get; set; }
        public decimal? HAVG { get; set; }
        public decimal? HOPS { get; set; }
        public decimal? PIP { get; set; }
        public int? PBB { get; set; }
        public int? PSO { get; set; }
        public decimal? PWHIP { get; set; }
        public string Size { get; set; }
        public string Throws { get; set; }
        public string Bats { get; set; }
        public string PlayerNote { get; set; }
    }

    [Route("/player", "PUT")]
    [Route("/player/{id}", "PUT")]
    public class UpdatePlayerRequest : IReturn<PlayerResponse>
    {
        public int Id { get; set; }
        public string PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        public int LeagueAge { get; set; }
        public string LeaguePlayed { get; set; }
        public string ASMI { get; set; }
        public int? HBB { get; set; }
        public int? HSO { get; set; }
        public decimal? HAVG { get; set; }
        public decimal? HOPS { get; set; }
        public decimal? PIP { get; set; }
        public int? PBB { get; set; }
        public int? PSO { get; set; }
        public decimal? PWHIP { get; set; }
        public string Size { get; set; }
        public string Throws { get; set; }
        public string Bats { get; set; }
        public string PlayerNote { get; set; }
    }

    [Route("/player", "DELETE")]
    [Route("/player/{id}", "DELETE")]
    public class DeletePlayerRequest
    {
        public int Id { get; set; }
    }

    public class PlayerResponse
    {
        public int Id { get; set; }
        public string PlayerNumber { get; set; }
        public string PlayerName { get; set; }
        public int LeagueAge { get; set; }
        public string LeaguePlayed { get; set; }
        public string ASMI { get; set; }
        public int? HBB { get; set; }
        public int? HSO { get; set; }
        public decimal? HAVG { get; set; }
        public decimal? HOPS { get; set; }
        public decimal? PIP { get; set; }
        public int? PBB { get; set; }
        public int? PSO { get; set; }
        public decimal? PWHIP { get; set; }
        public string Size { get; set; }
        public string Throws { get; set; }
        public string Bats { get; set; }
        public string PlayerNote { get; set; }
    }

    public class DeletePlayerRequestValidator : AbstractValidator<DeletePlayerRequest>
    {
        public DeletePlayerRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdatePlayerRequestValidator : AbstractValidator<UpdatePlayerRequest>
    {
        public UpdatePlayerRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class GetPlayerRequestValidator : AbstractValidator<GetPlayerRequest>
    {
        public GetPlayerRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreatePlayerRequestValidator : AbstractValidator<CreatePlayerRequest>
    {
        public CreatePlayerRequestValidator() {
            RuleFor(x => x.PlayerName).NotEmpty();
        }
    }
}