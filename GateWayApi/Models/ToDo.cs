using System;

namespace GateWayApi.Models
{
    public class ToDo{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isComplete { get; set; } = false;
    }
}