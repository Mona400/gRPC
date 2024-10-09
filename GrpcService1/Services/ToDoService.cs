using GrpcService1.Models;
using Grpc.Core;
using GrpcService1;
using ToDoGrpc;
using GrpcService1.Data;
using Microsoft.EntityFrameworkCore;
namespace GrpcService1.Services
{
    public class ToDoService : TodoIt.TodoItBase
    {
        private readonly AppDbContext _appDbContext;

        public ToDoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
        {
            if (request.Title == string.Empty || request.Describtion == String.Empty)
            {
                throw new Exception("please enter the object");
            }

            var todo = new ToDoItem
            {
                Describtion = request.Describtion,
                Title = request.Title,

            };
            await _appDbContext.ToDoItems.AddAsync(todo);
            await _appDbContext.SaveChangesAsync();

            return await Task.FromResult(new CreateToDoResponse
            {
                Id = todo.Id

            });
        }
        public override async Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
        {

            if (request.Id < 0)
            {
                throw new Exception("please enter the object");
            }
            var todoItem = _appDbContext.ToDoItems.FirstOrDefault(x => x.Id == request.Id);
            if (todoItem == null)
            {
                throw new Exception("please enter the object");
            }
            return await Task.FromResult(new ReadToDoResponse
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Describtion = todoItem.Describtion,
                ToDoStatus = todoItem.ToDoStatus,

            });


        }
        public override async Task<GetAllToDoResponse> ReadAll(GetAllToDoRequest request, ServerCallContext context)
        {
            var Response = new GetAllToDoResponse();
            var todoList = await _appDbContext.ToDoItems.ToListAsync();

            if (todoList == null)
            {
                throw new Exception("please enter the object");
            }
            foreach (var todo in todoList)
            {
                Response.ToDo.Add(new ReadToDoResponse
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Describtion = todo.Describtion,
                    ToDoStatus = todo.ToDoStatus,
                });
            }
            return await Task.FromResult(Response);
        }

        public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
        {
            var todo = await _appDbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (todo == null)
            {
                throw new Exception("the object is null");
            }

            todo.Id = request.Id;
            todo.Title = request.Title;
            todo.Describtion = request.Describtion;
            todo.ToDoStatus = request.ToDoStatus;
            await _appDbContext.SaveChangesAsync();

            return await Task.FromResult(new UpdateToDoResponse
            {
                Id = todo.Id,

            });
        }
        public override async Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
        {
            var todo = await _appDbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (todo == null)
            {
                throw new Exception("the object is null");
            }
            _appDbContext.ToDoItems.Remove(todo);
           await _appDbContext.SaveChangesAsync();
            return await Task.FromResult(new DeleteToDoResponse
            {
                Id = todo.Id,

            });
        }
    }
}
