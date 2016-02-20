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
    public class HittingService : SecureBaseService
    {
        public IHittingRepository hitting_repository { get; set; }

        public object Get(GetHittingsRequest request) {
            
            var data = hitting_repository.All();
            var response = data.Select(x => x.ConvertTo<HittingResponse>()).ToList();

            return response;
        }

        public object Get(GetHittingRequest request) {
            var data = hitting_repository.GetById(request.Id);
            var response = data.ConvertTo<HittingResponse>();

            return response;
        }

        public object Post(CreateHittingRequest request) {
            var data = request.ConvertTo<Hitting>();
            var id = hitting_repository.Insert(data);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return Get(new GetHittingRequest {Id = id});
        }

        public object Put(UpdateHittingRequest request) {
            var object_to_update = request.ConvertTo<Hitting>();
            hitting_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetHittingRequest {Id = request.Id});
        }

        public object Delete(DeleteHittingRequest request) {
            hitting_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/hitting", "GET")]
    public  class GetHittingsRequest : IReturn<HittingResponse>
    {
        
    }

    [Route("/hitting", "GET")]
    [Route("/hitting/{id}", "GET")]
    public class GetHittingRequest : IReturn<List<HittingResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/hitting", "POST")]
    public class CreateHittingRequest : IReturn<HittingResponse>
    {        
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Power { get; set; }
        public decimal? Contact { get; set; }
        public string HittingNote { get; set; }
    }

    [Route("/hitting", "PUT")]
    [Route("/hitting/{id}", "PUT")]
    public class UpdateHittingRequest : IReturn<HittingResponse>
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Power { get; set; }
        public decimal? Contact { get; set; }
        public string HittingNote { get; set; }
    }

    [Route("/hitting", "DELETE")]
    [Route("/hitting/{id}", "DELETE")]
    public class DeleteHittingRequest
    {
        public int Id { get; set; }
    }

    public class HittingResponse
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Power { get; set; }
        public decimal? Contact { get; set; }
        public string HittingNote { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DeleteHittingRequestValidator : AbstractValidator<DeleteHittingRequest>
    {
        public DeleteHittingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdateHittingRequestValidator : AbstractValidator<UpdateHittingRequest>
    {
        public UpdateHittingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.PlayerId).NotEmpty();
        }
    }

    public class GetHittingRequestValidator : AbstractValidator<GetHittingRequest>
    {
        public GetHittingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreateHittingRequestValidator : AbstractValidator<CreateHittingRequest>
    {
        public CreateHittingRequestValidator() {
            RuleFor(x => x.PlayerId).NotEmpty();
        }
    }
}