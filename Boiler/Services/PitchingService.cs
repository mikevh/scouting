using System;
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
    public class PitchingService : SecureBaseService
    {
        public IPitchingRepository pitching_repository { get; set; }

        public object Get(GetPitchingsRequest request) {
            
            var data = pitching_repository.All();
            var response = data.Select(x => x.ConvertTo<PitchingResponse>()).ToList();

            return response;
        }

        public object Get(GetPitchingRequest request) {
            var data = pitching_repository.GetById(request.Id);
            var response = data.ConvertTo<PitchingResponse>();

            return response;
        }

        public object Post(CreatePitchingRequest request) {
            var data = request.ConvertTo<Pitching>();
            var id = pitching_repository.Insert(data);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return Get(new GetPitchingRequest {Id = id});
        }

        public object Put(UpdatePitchingRequest request) {
            var object_to_update = request.ConvertTo<Pitching>();
            pitching_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetPitchingRequest {Id = request.Id});
        }

        public object Delete(DeletePitchingRequest request) {
            pitching_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/pitching", "GET")]
    public  class GetPitchingsRequest : IReturn<PitchingResponse>
    {
        
    }

    [Route("/pitching", "GET")]
    [Route("/pitching/{id}", "GET")]
    public class GetPitchingRequest : IReturn<List<PitchingResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/pitching", "POST")]
    public class CreatePitchingRequest : IReturn<PitchingResponse>
    {
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Velocity { get; set; }
        public decimal? Command { get; set; }
        public string PitchingNote { get; set; }
    }

    [Route("/pitching", "PUT")]
    [Route("/pitching/{id}", "PUT")]
    public class UpdatePitchingRequest : IReturn<PitchingResponse>
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Velocity { get; set; }
        public decimal? Command { get; set; }
        public string PitchingNote { get; set; }
    }

    [Route("/pitching", "DELETE")]
    [Route("/pitching/{id}", "DELETE")]
    public class DeletePitchingRequest
    {
        public int Id { get; set; }
    }

    public class PitchingResponse
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Velocity { get; set; }
        public decimal? Command { get; set; }
        public string PitchingNote { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DeletePitchingRequestValidator : AbstractValidator<DeletePitchingRequest>
    {
        public DeletePitchingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdatePitchingRequestValidator : AbstractValidator<UpdatePitchingRequest>
    {
        public UpdatePitchingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.PlayerId).NotEmpty();
        }
    }

    public class GetPitchingRequestValidator : AbstractValidator<GetPitchingRequest>
    {
        public GetPitchingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreatePitchingRequestValidator : AbstractValidator<CreatePitchingRequest>
    {
        public CreatePitchingRequestValidator() {
            RuleFor(x => x.PlayerId).NotEmpty();
        }
    }
}