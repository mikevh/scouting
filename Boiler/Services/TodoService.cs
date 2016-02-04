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
    public class TodoService : SecureBaseService
    {
        public ITodoRepository todo_repository { get; set; }

        public object Get(GetTodosRequest request) {
            
            var data = todo_repository.All();
            var response = data.Select(x => x.ConvertTo<TodoResponse>()).ToList();

            return response;
        }

        public object Get(GetTodoRequest request) {
            var data = todo_repository.GetById(request.Id);
            var response = data.ConvertTo<TodoResponse>();

            return response;
        }

        public object Post(CreateTodoRequest request) {
            var data = request.ConvertTo<Todo>();
            var id = todo_repository.Insert(data);
            Response.StatusCode = (int)HttpStatusCode.Created;

            return Get(new GetTodoRequest {Id = id});
        }

        public object Put(UpdateTodoRequest request) {
            var object_to_update = request.ConvertTo<Todo>();
            todo_repository.Update(object_to_update);

            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Get(new GetTodoRequest {Id = request.Id});
        }

        public object Delete(DeleteTodoRequest request) {
            todo_repository.Delete(request.Id);
            return new HttpResult { StatusCode = HttpStatusCode.Accepted };
        }
    }

    [Route("/todo", "GET")]
    public  class GetTodosRequest : IReturn<TodoResponse>
    {
        
    }

    [Route("/todo", "GET")]
    [Route("/todo/{id}", "GET")]
    public class GetTodoRequest : IReturn<List<TodoResponse>>
    {
        public int Id { get; set; }
    }

    [Route("/todo", "POST")]
    public class CreateTodoRequest : IReturn<TodoResponse>
    {
        public string Name { get; set; }
        public bool IsDone { get; set; }
    }

    [Route("/todo", "PUT")]
    [Route("/todo/{id}", "PUT")]
    public class UpdateTodoRequest : IReturn<TodoResponse>
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool IsDone { get; set; }
    }

    [Route("/todo", "DELETE")]
    [Route("/todo/{id}", "DELETE")]
    public class DeleteTodoRequest
    {
        public int Id { get; set; }
    }

    public class TodoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
    }

    public class DeleteTodoRequestValidator : AbstractValidator<DeleteTodoRequest>
    {
        public DeleteTodoRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
    {
        public UpdateTodoRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class GetTodoRequestValidator : AbstractValidator<GetTodoRequest>
    {
        public GetTodoRequestValidator() {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
    {
        public CreateTodoRequestValidator() {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}