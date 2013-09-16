using Applicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicate.Data
{
    public class DataPersister
    {
        protected static string AccessToken { get; set; }

        private const string BaseServicesUrl = "http://localhost:16183/api/";

        internal static void RegisterUser(string username, string email, string authenticationCode)
        {
            //Validation!!!!!
            //validate username
            //validate email
            //validate authentication code
            //use validation from WebAPI
            var userModel = new UserModel()
            {
                Username = username,
                Email = email,
                AuthCode = authenticationCode
            };
            var httpRequester = new HttpRequester(BaseServicesUrl);

            var response = httpRequester.Post<UserModel>("users/register",userModel);
        }

        internal static string LoginUser(string username, string authenticationCode)
        {
            //Validation!!!!!
            //validate username
            //validate authentication code
            //use validation from WebAPI
            var userModel = new UserModel()
            {
                Username = username,
                AuthCode = authenticationCode
            };

            var httpRequester = new HttpRequester(BaseServicesUrl);

            var loginResponse = httpRequester
                .Post<LoginResponseModel>("auth/token", userModel);

            AccessToken = loginResponse.AccessToken;
            return loginResponse.Username;
        }

        internal static bool LogoutUser()
        {
            var headers = new Dictionary<string, string>();
            headers["X-accessToken"] = AccessToken;
            var httpRequester = new HttpRequester(BaseServicesUrl);
            var isLogoutSuccessful = httpRequester.Put("users/logout", headers);
            return isLogoutSuccessful;
        }

        //internal static void CreateNewTodosList(string title, IEnumerable<TodoViewModel> todos)
        //{
        //    var listModel = new TodolistModel()
        //    {
        //        Title = title,
        //        Todos = todos.Select(t => new TodoModel()
        //        {
        //            Text = t.Text
        //        })
        //    };

        //    var headers = new Dictionary<string, string>();
        //    headers["X-accessToken"] = AccessToken;

        //    var httpRequester = new HttpRequester(BaseServicesUrl);

        //    var response =
        //        httpRequester.Post<ListCreatedModel>("lists", listModel, headers);
        //}

        //internal static IEnumerable<TodoListViewModel> GetTodoLists()
        //{
        //    var headers = new Dictionary<string, string>();
        //    headers["X-accessToken"] = AccessToken;

        //    var httpRequester = new HttpRequester(BaseServicesUrl);

        //    var todoListsModels =
        //        httpRequester.Get<IEnumerable<TodolistModel>>("lists", headers);

        //    return todoListsModels.
        //        AsQueryable().
        //         Select(model => new TodoListViewModel()
        //          {
        //              Id = model.Id,
        //              Title = model.Title,
        //              Todos = model.Todos.AsQueryable().Select(todo => new TodoViewModel()
        //              {
        //                  Id = todo.Id,
        //                  Text = todo.Text,
        //                  IsDone = todo.IsDone
        //              })
        //          });
        //}

        //internal static void ChangeState(int todoId)
        //{
        //    var headers = new Dictionary<string, string>();
        //    headers["X-accessToken"] = AccessToken;

        //    var httpRequester = new HttpRequester(BaseServicesUrl);

        //    httpRequester.Put("todos/" + todoId, headers);
        //}

        //internal static IEnumerable<AppointmentViewModel> GetAllAppointments()
        //{
        //    var headers = new Dictionary<string, string>();
        //    headers["X-accessToken"] = AccessToken;

        //    var httpRequester = new HttpRequester(BaseServicesUrl);

        //    return httpRequester.
        //        Get<IEnumerable<AppointmentViewModel>>("appointments/all", headers);
        //}

        //internal static AppointmentModel AddNewAppointment(string subject,
        //    string description, DateTime appointmentDate, int duration)
        //{
        //    var appointment = new AppointmentModel
        //    {
        //        Subject = subject,
        //        Description = description,
        //        AppointmentDate = appointmentDate.ToString(),
        //        Duration = duration
        //    };

        //    var headers = new Dictionary<string, string>();
        //    headers["X-accessToken"] = AccessToken;

        //    var httpRequester = new HttpRequester(BaseServicesUrl);

        //    var response = httpRequester.Post<AppointmentModel>(
        //        "appointments/new",
        //        appointment, 
        //        headers);

        //    return response;
        //}
    }
}