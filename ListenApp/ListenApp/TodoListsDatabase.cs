using SQLite;

namespace ListenApp;

// following the MVVM principle the Model represents the business logic and data objects

// The TodoItemDatabase uses asynchronous lazy initialization to delay initialization of the database until it's first accessed,
// with a simple Init method that gets called by each method in the class:
public class TodoListsDatabase
{
    SQLiteAsyncConnection Database;

    public TodoListsDatabase()
    {
        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        Database.CreateTableAsync<Lists>();
        Database.CreateTableAsync<Tasks>();
    }
    // methods for the four types of data manipulation: create, read, update and delete (CRUD) for the Lists-Table
    // async Task Init()
    // {
    //     if (Database is not null)
    //         return;
    //
    //     Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    //     var resultlist = await Database.CreateTableAsync<Lists>();
    //     var resulttask = await Database.CreateTableAsync<Tasks>();
    // }

    // receiving all Lists-Items ordered by their PositinoNumber
    public async Task<List<Lists>> GetItemsAsync()
    {
        return await Database.Table<Lists>().OrderBy(i => i.PositionNumber).ToListAsync();
    }

    // receiving all Lists-Items which are not completed yet
    public async Task<List<Lists>> GetItemNotDoneAsync()
    {
        return await Database.Table<Lists>().Where(t => t.IsCompleted).ToListAsync();
        
        // SQL queries are also possible
        // return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    }
    
    // receiving a single Lists-Item by its ID
    public async Task<Lists> GetItemAsync(int id)
    {
        return await Database.Table<Lists>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    // store changes in the database
    public async Task<int> UpdateItemAsync(Lists item)
    {
        
        return await Database.UpdateAsync(item);
    }
    
    // store item in database
    public async Task<int> SaveItemAsync(Lists item)
    {
        if (item.Id != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }

    // delete item in database
    public async Task<int> DeleteItemAsync(Lists item)
    {
        return await Database.DeleteAsync(item);
    }
    
    // CRUD-Operations for the Tasks-Table
    
    // nur eine Init-Methode
    // async Task InitTasks()
    // {
    //     if (Database is not null)
    //         return;
    //
    //     // ??? Do I really need to repeat this line of code: see line 20 
    //     Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags); 
    //     var result = await Database.CreateTableAsync<Tasks>();
    // }

    // receiving all Tasks-Items ordered by their PositinoNumber
    public async Task<List<Tasks>> GetItemsAsyncTasks(int id)
    {
        // return await Database.Table<Tasks>().ToListAsync();
        return await Database.Table<Tasks>().Where(i => i.IdLists == id).OrderBy(i => i.PositionNumber).ToListAsync();
    }
    
    // receiving all Tasks-Items whose title contains the search text
    public async Task<List<Tasks>> GetItemsAsyncTasksSearch(int id, string s)
    {
        // return await Database.Table<Tasks>().ToListAsync();
        // return await Database.Table<Tasks>().Where(i => i.IdLists == id && i.TitleTasks == "%" + search + "%").ToListAsync();
        return await Database.Table<Tasks>()
            .Where(i => i.IdLists == id && i.TitleTasks.ToLower().Contains(s.ToLower()))
            .ToListAsync();

    }
    
    // receiving the list of the Tasks-Items beginning with the latest item stored
    // public async Task<List<Tasks>> GetItemsAsyncTasksAsc(int id)
    // {
    //     return await Database.Table<Tasks>().Where(i => i.IdLists == id).OrderBy(x => x.DateTasks).ToListAsync();
    // }
    
    // receiving the list of the Tasks-Items beginning with the first item stored
    public async Task<List<Tasks>> GetItemsAsyncTasksDesc(int id)
    {
        return await Database.Table<Tasks>().Where(i => i.IdLists == id).OrderByDescending(x => x.DateTasks).ToListAsync();
    }

    // receiving all Tasks-Items which are not completed yet
    public async Task<List<Tasks>> GetItemNotDoneAsyncTasks(int id)
    {
        return await Database.Table<Tasks>().Where(t => t.IdLists == id && t.IsCompletedTasks == false).ToListAsync();

        // SQL queries are also possible
        // return await Database.QueryAsync<Tasks>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
    }

    // receiving a single Lists-Item by its ID
    public async Task<Tasks> GetItemAsyncTasks(int id)
    {
        return await Database.Table<Tasks>().Where(i => i.IdTasks == id).FirstOrDefaultAsync();
    }

    // store changes in the database
    public async Task<int> UpdateItemAsyncTasks(Tasks item)
    {
        
        return await Database.UpdateAsync(item);
    }
    
    // store item in database
    public async Task<int> SaveItemAsyncTasks(Tasks item)
    {
        if (item.IdTasks != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }

    // delete item in database
    public async Task<int> DeleteItemAsyncTasks(Tasks item)
    {
        return await Database.DeleteAsync(item);
    }
}