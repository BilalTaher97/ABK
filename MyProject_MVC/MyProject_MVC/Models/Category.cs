using System;
using System.Collections.Generic;

namespace MyProject_MVC.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();
}
