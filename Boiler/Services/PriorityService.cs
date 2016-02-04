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
    public class PriorityService : SecureBaseService
    {
        public IPriorityRepository priority_repository { get; set; }

        public object Get(GetPrioritysRequest request) {
            
            var data = priority_repository.All();
            var response = data.Select(x => x.ConvertTo<PriorityResponse>()).ToList();

            return response;
        }

        public object Get(GetPriorityRequest request) {
            var data = priority_repository.GetById(request.Id);
            var response = data.ConvertTo<PriorityResponse>();

            return response;
        }

        public object Post(CreatePriorityRequest request) {
            var data = request.ConvertTo<Priority>();
            var id = priority_repository.Insert(data);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return Get(new GetPriorityRequest {Id = id});
        }

        public object Put(UpdatePriorityRequest request) {
            var object_to_update = request.ConvertTo<Priority>();
            priority_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetPriorityRequest {Id = request.Id});
        }

        public object Delete(DeletePriorityRequest request) {
            priority_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/priority", "GET")]
    public  class GetPrioritysRequest : IReturn<PriorityResponse>
    {
        
    }

    [Route("/priority", "GET")]
    [Route("/priority/{id}", "GET")]
    public class GetPriorityRequest : IReturn<List<PriorityResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/priority", "POST")]
    public class CreatePriorityRequest : IReturn<PriorityResponse>
    {
        public string Name { get; set; }
        public int Ordinal { get; set; }
    }

    [Route("/priority", "PUT")]
    [Route("/priority/{id}", "PUT")]
    public class UpdatePriorityRequest : IReturn<PriorityResponse>
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool Ordinal { get; set; }
    }

    [Route("/priority", "DELETE")]
    [Route("/priority/{id}", "DELETE")]
    public class DeletePriorityRequest
    {
        public int Id { get; set; }
    }

    public class PriorityResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
    }

    public class DeletePriorityRequestValidator : AbstractValidator<DeletePriorityRequest>
    {
        public DeletePriorityRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdatePriorityRequestValidator : AbstractValidator<UpdatePriorityRequest>
    {
        public UpdatePriorityRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Ordinal).NotEmpty();
        }
    }

    public class GetPriorityRequestValidator : AbstractValidator<GetPriorityRequest>
    {
        public GetPriorityRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreatePriorityRequestValidator : AbstractValidator<CreatePriorityRequest>
    {
        public CreatePriorityRequestValidator() {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Ordinal).NotEmpty();
        }
    }
}