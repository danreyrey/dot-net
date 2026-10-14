using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace ListenApp;

// following the MVVM principle the Model represents the business logic and data objects
public class Tasks
{
    [PrimaryKey, AutoIncrement]
    public int IdTasks{ get; set; }

    public string TitleTasks { get; set; }

    public bool IsCompletedTasks { get; set; }
    
    public DateTime DateTasks { get; set; }
    
    // with the Foreign-Key I can relate two database-tables to each other (Lists and Tasks)
    [ForeignKey("Id")]
    public int IdLists { get; set; }

    public int PositionNumber { get; set; }
}