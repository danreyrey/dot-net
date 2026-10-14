using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ListenApp.ViewModel;

// following the MVVM principle the ViewModel serves as a Proxy and is responsible for the flow of interaction and which data to display
public partial class MainViewModel : ObservableObject
{
    public MainViewModel(TodoListsDatabase database) // dependency injection to inject the database in the MainViewModel
    {
        Items = new ObservableCollection<Lists>();
        _database = database;
    }

    private TodoListsDatabase _database;
    
    // string-based query parameter in order to get all the tasks related to the list whose ID has been submitted
    int idList;
    
    // represents a dynamic data collection that provides notifications when items get added or removed,
    // or when the whole list is refreshed
    [ObservableProperty]
    ObservableCollection<Lists> items;
    
    // boolean variable for enabling and disabling the Edit-Method
    [ObservableProperty] 
    private bool isEditEnabled = false;
    
    // boolean variable for enabling and disabling the Swipe-Items during drag-and-drop-operations
    [ObservableProperty] 
    private bool isSwipeViewEnabled = true;
    
    // entry text which defines the list's title
    [ObservableProperty]
    private string text;
    
    // add item to the database and to the list "Items"
    [RelayCommand]
    private async Task Add() // Refresh ObservableCollection in order to get the ID of the new element which was stored in the database
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;
        // declare and initialize the variable list to store the all Titles (names of the ToDoLists) received from the database table Lists in this variable
        var list = new Lists(){Title = Text, PositionNumber = Items.Count + 1};
        await _database.SaveItemAsync(list);
        
        Items.Add(list);
        // add out item
        Text = string.Empty;
    }

    // delete item from the database and from the list "Items"
    [RelayCommand]
    private async Task Delete(Lists item)
    {
        if (Items.Contains(item))
        {
            await _database.DeleteItemAsync(item);
            Items.Remove(item);
        }
    }

    // navigate to DetailPage by tapping on the item
    [RelayCommand]
    private async Task Tap(Lists item)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?id={item.Id}");
    }

    //  receive all elements of the Lists-Database-Table
    public async Task OnAppearing()
    {
        var lists = await _database.GetItemsAsync();
        Items.Clear();
        // in order to receive all "Titles" from Lists and add them to Items
        foreach (var item in lists)
        {
            Items.Add(item);
        }
    }
    
    // storing the edited item in the database
    [RelayCommand]
    private async Task EditList()
    {
        if (string.IsNullOrWhiteSpace(Text)) 
            return;
        
        var item = await _database.GetItemAsync(idList);
        item.Title = Text;
        await _database.UpdateItemAsync(item);

        
        Text = string.Empty;
        idList = 0;
        IsEditEnabled = !IsEditEnabled;
        Items.Clear();
        OnAppearing();
    }

    // enabling the Edit-Method
    [RelayCommand]
    private void EnableEdit(Lists item)
    {
        IsEditEnabled = !IsEditEnabled;
        idList = item.Id;
        Text = item.Title;
    }
    
    // storing the new position of the item in the database
    public async void EditListPosition(int oldPosition, int newPosition)
    {
        var lists = await _database.GetItemsAsync();

        if (oldPosition > newPosition)
        {
            for (int i = newPosition; i <= oldPosition; i++)
            {
                var item = lists[i];;
                
                if (i == oldPosition)
                {
                    item.PositionNumber = newPosition + 1;
                    await _database.UpdateItemAsync(item);
                }
                else
                {
                    item.PositionNumber = i + 2;
                    await _database.UpdateItemAsync(item);
                }    
            }    
        } 
        else if (oldPosition < newPosition)
        {
            for (int i = oldPosition; i <= newPosition; i++)
            {
                var item = lists[i];;
                
                if (i == oldPosition)
                {
                    item.PositionNumber = newPosition + 1;
                    await _database.UpdateItemAsync(item);
                }
                else
                {
                    item.PositionNumber = i;
                    await _database.UpdateItemAsync(item);
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

}