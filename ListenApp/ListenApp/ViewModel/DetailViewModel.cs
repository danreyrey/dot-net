using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Plugin.LocalNotification;

namespace ListenApp.ViewModel;

// following the MVVM principle the ViewModel serves as a Proxy and is responsible for the flow of interaction and which data to display
public partial class DetailViewModel : ObservableObject
{
    public DetailViewModel(TodoListsDatabase database) // dependency injection to inject the database in the MainViewModel
    {
        Items1 = new ObservableCollection<Tasks>();
        _database = database;
    }
    
    private TodoListsDatabase _database;

    // variable that indicates which item is being edited identified by the id
    int idTask;
    
    // represents a dynamic data collection that provides notifications when items get added or removed,
    // or when the whole list is refreshed
    [ObservableProperty] ObservableCollection<Tasks> items1;

    // entry text which defines the task's title
    [ObservableProperty] private string text1;

    // string-based query parameter in order to get all the tasks related to the list whose ID has been submitted
    [ObservableProperty] 
    int idList;

    // variable representing the DetailsPage's headline
    [ObservableProperty] 
    private string headline;

    // boolean variable for enabling and disabling the edit-method
    [ObservableProperty] 
    private bool isEditEnabled = false;
    
    // boolean variable for enabling and disabling the Swipe-Items during drag-and-drop-operations
    [ObservableProperty] 
    private bool isSwipeViewEnabled = true;
    
    // boolean variable for enabling and disabling the switch
    [ObservableProperty] 
    private bool isSwitchEnabled = true;
    
    // navigating back to the Main-Page
    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync(".."); // Navigating back to MainPage
    }
    
    // add item to the database and to the list "Items1"
    [RelayCommand]
    private async Task Add()
    {
        if (string.IsNullOrWhiteSpace(Text1)) 
            return;
        
        var task = new Tasks(){TitleTasks = Text1, DateTasks = DateTime.Now, IdLists = IdList, PositionNumber = Items1.Count + 1}; // Connected Foreign-Key (Tasks) and Primary-Key (Lists) to relate the tables to each other
        await _database.SaveItemAsyncTasks(task);
        
        Items1.Add(task);
        // add out item
        Text1 = string.Empty;
    }

    // delete item from the database and from the list "Items1"
    [RelayCommand]
    private async Task Delete(Tasks item)
    {
        if (Items1.Contains(item))
        {
            await _database.DeleteItemAsyncTasks(item);
            Items1.Remove(item);
        }
    }
    
    // receiving the list's title which is data bound to the Detail-Page's title
    public async Task GetListTitle()
    {
        var item = await _database.GetItemAsync(IdList);
        Headline = item.Title;
    }

    //  receive all elements of the Tasks-Database-Table 
    public async Task GetTaskTitle()
    {
        var tasks = await _database.GetItemsAsyncTasks(IdList);
        // in order to receive all "Titles" from Tasks and add them to Items
        foreach (var item in tasks)
        {
            Items1.Add(item);
        }
    }

    // public async Task GetTaskTitleByDateAsc()
    // {
    //     var tasks = await _database.GetItemsAsyncTasksAsc(IdList);
    //     Items1.Clear();
    //     // in order to receive all "Titles" from Tasks and add them to Items
    //     foreach (var item in tasks)
    //     {
    //         Items1.Add(item);
    //     }
    // }
    
    //  receive all elements of the Tasks-Database-Table starting with the most recently added
    public async Task GetTaskTitleByDateDesc()
    {
        var tasks = await _database.GetItemsAsyncTasksDesc(IdList);
        // in order to receive all "Titles" from Tasks and add them to Items
        Items1.Clear();
        foreach (var item in tasks)
        {
            Items1.Add(item);
        }
    }
    
    //  receive all elements of the Tasks-Database-Table whose title contains the search text
    public async Task GetItemsTasksSearch(string s)
    {
        var tasks = await _database.GetItemsAsyncTasksSearch(IdList, s);
        // in order to receive all "Titles" from Tasks and add them to Items
        Items1.Clear();
        foreach (var item in tasks)
        {
            Items1.Add(item);
        }
    }
    
    // similar to OnDisappearing but keeping the headline
    public void OnDisappearingKeepingHeadline() 
    {
        Items1.Clear();
    }
    
    // to avoid errors when navigating to the Detail-Page again clear the list Items1 and the headline
    public void OnDisappearing() 
    {
        Items1.Clear();
        Headline = "";
    }

    // storing the edited item in the database
    [RelayCommand]
    private async Task EditTask()
    {
        if (string.IsNullOrWhiteSpace(Text1)) 
            return;
        
        var item = await _database.GetItemAsyncTasks(idTask);
        item.TitleTasks = Text1;
        await _database.UpdateItemAsyncTasks(item);

        
        Text1 = string.Empty;
        idTask = 0;
        IsEditEnabled = !IsEditEnabled;
        Items1.Clear();
        GetTaskTitle();
    }
    
    // enabling the edit-method
    [RelayCommand]
    private void EnableEdit(Tasks item)
    {
        IsEditEnabled = !IsEditEnabled;
        idTask = item.IdTasks;
        Text1 = item.TitleTasks;
    }
    
    // is called when the checkbox is being clicked and stores the updated information in the database
    public async void CheckBoxChanged(int id, bool value)
    {
        var items = await _database.GetItemAsyncTasks(id);
        if (items.IsCompletedTasks != value)
        {
            items.IsCompletedTasks = value;
            await _database.UpdateItemAsyncTasks(items);
            Items1.Clear();
            GetTaskTitle();
        }
    }

    // method handling the local notifications
    public async void ShowNotification()
    {
        var tasks = await _database.GetItemNotDoneAsyncTasks(IdList);
        int counter = tasks.Count;
        
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        if (counter != 0)
        {
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "You still got to complete " + counter + " tasks!",
                Description = "DO IT!!!",
                Android =
                {
                    IconSmallName =
                    {
                        ResourceName = "nct",
                    }            
                },
                // ReturningData = "abc", // Returning data when tapped on notification.
                Schedule = 
                {
                    NotifyTime = DateTime.Now.AddSeconds(5), // This is Used for Scheduling local notifications; if not specified, the notification will show immediately.
                    NotifyRepeatInterval = TimeSpan.FromDays(1)
                }
            };
            await LocalNotificationCenter.Current.Show(notification);
        }
        
        // var request = new NotificationRequest
        // {
        //     ...
        //
        //     // Android = new AndroidOptions()
        //     // {
        //     //     // VibrationPattern = 
        //     //     // LedColor = 
        //     //     // etc. (Android-specific options)
        //     // },
        //     // iOS = new Plugin.LocalNotification.iOSOption.iOSOptions
        //     // {
        //     //     // ApplyBadgeValue = 
        //     //     // HideForegroundAlert = 
        //     //     // etc. (iOS-specific options)
        //     // }
        // };
        // LocalNotificationCenter.Current.Show(request);
    }
    
    // storing the new position of the item in the database
    public async Task EditTaskPosition(int oldPosition, int newPosition, int idList)
    {
        var tasks = await _database.GetItemsAsyncTasks(idList);

        if (oldPosition > newPosition)
        {
            for (int i = newPosition; i <= oldPosition; i++)
            {
                var item = tasks[i];;
                
                if (i == oldPosition)
                {
                    item.PositionNumber = newPosition + 1;
                    await _database.UpdateItemAsyncTasks(item);
                }
                else
                {
                    item.PositionNumber = i + 2;
                    await _database.UpdateItemAsyncTasks(item);
                }    
            }    
        } 
        else if (oldPosition < newPosition)
        {
            for (int i = oldPosition; i <= newPosition; i++)
            {
                var item = tasks[i];;
                
                if (i == oldPosition)
                {
                    item.PositionNumber = newPosition + 1;
                    await _database.UpdateItemAsyncTasks(item);
                }
                else
                {
                    item.PositionNumber = i;
                    await _database.UpdateItemAsyncTasks(item);
                }
            } 
        }   
    }
    
    // enabling and disabling the SwipeView/Swipe-Items during drag-and-drop-operations
    public bool EnableSwipeView()
    {
        return IsSwipeViewEnabled = true;
    }
    
    public bool DisableSwipeView()
    {
        return IsSwipeViewEnabled = false;
    }
    
    // enabling and disabling the switch while searching for tasks using the SearchBar
    public bool EnableSwitch()
    {
        return IsSwitchEnabled = true;
    }
    
    public bool DisableSwitch()
    {
        return IsSwitchEnabled = false;
    }
}
