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
    public class FieldingService : SecureBaseService
    {
        public IFieldingRepository fielding_repository { get; set; }

        public object Get(GetFieldingsRequest request) {
            
            var data = fielding_repository.All();
            var response = data.Select(x => x.ConvertTo<FieldingResponse>()).ToList();

            return response;
        }

        public object Get(GetFieldingRequest request) {
            var data = fielding_repository.GetById(request.Id);
            var response = data.ConvertTo<FieldingResponse>();

            return response;
        }

        public object Post(CreateFieldingRequest request) {
            var data = request.ConvertTo<Fielding>();
            var id = fielding_repository.Insert(data);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return Get(new GetFieldingRequest {Id = id});
        }

        public object Put(UpdateFieldingRequest request) {
            var object_to_update = request.ConvertTo<Fielding>();
            fielding_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetFieldingRequest {Id = request.Id});
        }

        public object Delete(DeleteFieldingRequest request) {
            fielding_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/fielding", "GET")]
    public  class GetFieldingsRequest : IReturn<FieldingResponse>
    {
        
    }

    [Route("/fielding", "GET")]
    [Route("/fielding/{id}", "GET")]
    public class GetFieldingRequest : IReturn<List<FieldingResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/fielding", "POST")]
    public class CreateFieldingRequest : IReturn<FieldingResponse>
    {
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Range { get; set; }
        public decimal? Hands { get; set; }
        public decimal? ArmStrength { get; set; }
        public string FieldingNote { get; set; }
    }

    [Route("/fielding", "PUT")]
    [Route("/fielding/{id}", "PUT")]
    public class UpdateFieldingRequest : IReturn<FieldingResponse>
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Range { get; set; }
        public decimal? Hands { get; set; }
        public decimal? ArmStrength { get; set; }
        public string FieldingNote { get; set; }
    }

    [Route("/fielding", "DELETE")]
    [Route("/fielding/{id}", "DELETE")]
    public class DeleteFieldingRequest
    {
        public int Id { get; set; }
    }

    public class FieldingResponse
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Range { get; set; }
        public decimal? Hands { get; set; }
        public decimal? ArmStrength { get; set; }
        public string FieldingNote { get; set; }
    }

    public class DeleteFieldingRequestValidator : AbstractValidator<DeleteFieldingRequest>
    {
        public DeleteFieldingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdateFieldingRequestValidator : AbstractValidator<UpdateFieldingRequest>
    {
        public UpdateFieldingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.PlayerId).NotEmpty();
        }
    }

    public class GetFieldingRequestValidator : AbstractValidator<GetFieldingRequest>
    {
        public GetFieldingRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreateFieldingRequestValidator : AbstractValidator<CreateFieldingRequest>
    {
        public CreateFieldingRequestValidator() {
            RuleFor(x => x.PlayerId).NotEmpty();
        }
    }
}