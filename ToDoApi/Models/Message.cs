using System;

namespace ToDoApi.Models{
    public class Message{
        public string code { get; set; }
        public string type { get; set; }
        public ToDo item {get; set;}
    }
}