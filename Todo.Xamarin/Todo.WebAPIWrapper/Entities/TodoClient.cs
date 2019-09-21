using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Todo.WebAPIWrapper.Entities
{
    public class TodoClient
    {
        private const string Api = "https://api.drax.codes/api/todo";
        private const string UserName = "daniel";
        private const string Password = "daniel";

        private readonly HttpClient _client;

        public TodoClient(HttpClient client = null)
        {
            // TODO: Refactor Httpclients and whatnots

            _client = client;

            if (_client != null) { return; }

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                ASCIIEncoding.ASCII.GetBytes($"{UserName}:{Password}")));

        }

        public Tasks Tasks { get => GetOrRefreshTodos().Result; }

        public async Task<Tasks> GetOrRefreshTodos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Api}/list");
            var httpResponse = await _client.SendAsync(request).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            var json = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Tasks>(json);
        }

        public async Task AddTodoAsync(TodoTask todo)
        {
            var json = JsonConvert.SerializeObject(todo);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{Api}/add");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTodosAsync(TodoTask[] todos)
        {
            var json = JsonConvert.SerializeObject(todos);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{Api}/removemany");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTodoAsync(TodoTask todo)
        {
            var json = JsonConvert.SerializeObject(todo);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{Api}/remove");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
