using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Todo.WebAPIWrapper.Entities
{
    public class TodoClient
    {
        private const string Api = "Api.drax.codes/Api/{0}";
        private const string UserName = "daniel";
        private const string PassWord = "daniel";

        private readonly HttpClient _client;

        public TodoClient(HttpClient client)
        {
            // TODO: Refactor Httpclients and whatnots

            _client = client;

            if (_client == null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"{UserName}:{PassWord}");
            }
        }

        public TodoTask[] Tasks { get => GetOrRefreshTodos().Result; }

        public async Task<TodoTask[]> GetOrRefreshTodos()
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{string.Format(Api, "todo/list")}");
            var httpResponse = await _client.SendAsync(httpRequest);

            httpResponse.EnsureSuccessStatusCode();

            var json = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TodoTask[]>(json);
        }

        public async Task AddTodoAsync(TodoTask todo)
        {
            var json = JsonConvert.SerializeObject(todo);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{string.Format(Api, "todo/add")}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTodosAsync(TodoTask[] todos)
        {
            var json = JsonConvert.SerializeObject(todos);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{string.Format(Api, "todo/removemany")}");
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
        }
    }
}
