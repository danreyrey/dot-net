using SQLite;

namespace ListenApp;

// following the MVVM principle the Model represents the business logic and data objects
public class Lists
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public int PositionNumber { get; set; }
}