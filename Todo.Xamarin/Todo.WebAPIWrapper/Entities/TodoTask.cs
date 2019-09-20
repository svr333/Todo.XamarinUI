using System;
using Newtonsoft.Json;

namespace Todo.WebAPIWrapper.Entities
{
    public class TodoTask
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }
        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }
    }

    public class Tasks
    {
        [JsonProperty("tasks")]
        public TodoTask[] Items { get; set; }
    }
}
