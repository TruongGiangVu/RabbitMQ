using System;
using Newtonsoft.Json;

namespace ToDoApi.Models
{
    public class ToDo{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isComplete { get; set; } = false;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}