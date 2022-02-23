using System;
using Newtonsoft.Json;

namespace ToDoApi.Models{
    public class Message{
        public string code { get; set; }
        public string type { get; set; }
        public ToDo item {get; set;}
        
        public override string ToString(){
            return JsonConvert.SerializeObject(this);
        }
    }
}