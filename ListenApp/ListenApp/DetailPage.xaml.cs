using ListenApp.ViewModel;
using DevExpress.Maui.CollectionView;

// following the MVVM principle the View's purpose is how to display information
namespace ListenApp;

// [QueryProperty("IdList", "id")] submitted ID of the Lists-Table
// which is used to receive all the Tasks-Items for the DetailPage
[QueryProperty(nameof(IdList), "id")]
public partial class DetailPage : ContentPage
{
    public DetailPage(DetailViewModel vm) // dependency injection to inject the DetailViewModel in the DetailPage
    {
        InitializeComponent(); // initializes the controls
        BindingContext = vm; // setting the BindingContext for the DetailPage to the ViewModel
    }

    private int newPosition; // in order to store the current final position of the CollectionView-Element that is dragged before it is dropped
    
    private string _idList; // in order to store the string-based query parameter send via URL

    public string IdList // in order to store the string-based query parameter send via URL
    {
        get => _idList;
        set
        {
            _idList = value;

            if (value != null)
            {
                ((DetailViewModel)BindingContext).IdList = int.Parse(value);
                ((DetailViewModel)BindingContext).GetListTitle(); // title of Detail-Page
                ((DetailViewModel)BindingContext).GetTaskTitle(); // OnAppearing of Detail-Page
                ((DetailViewModel)BindingContext).ShowNotification(); // local notification
                // line 33: Load Tasks-Elements (make sure that id has been set, before Lists-Data will be loaded)
            }
        }
    }

    // Clearing Items-List and Headline by calling the OnDisappearing-Method in order
    // to avoid display errors because of wrong data that's been taken from cache.
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((DetailViewModel)BindingContext).OnDisappearing();
        // Empty Items-List and Headline (string)
    }
    
    // passing the element's ID and it's completion status into the CheckBoxChanged-Method to store the updated value in the database
    void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var item = ((CheckBox)sender).BindingContext as Tasks;
        // Null-Reference-exception in case the item is null
        if (item == null) return;
        var itemId = item.IdTasks;
        var isChecked = e.Value;
        ((DetailViewModel)BindingContext).CheckBoxChanged(itemId, isChecked);
    }

    // void OnToggled(object sender, ToggledEventArgs e)
    // {
    //     // OnAppearing with Order By [Date] ASC
    //     var isToggled = e.Value;
    //     if (isToggled == true)
    //     {
    //         ((DetailViewModel)BindingContext).GetTaskTitleByDateAsc();
    //     }
    //     else
    //     {
    //         ((DetailViewModel)BindingContext).OnDisappearingKeepingHeadline();
    //         ((DetailViewModel)BindingContext).GetTaskTitle();
    //     }
    // }

    // method is called when the switch is being flipped to reorder the items by date
    void OnToggledDesc(object sender, ToggledEventArgs e)
    {
        // OnAppearing with Order By [Date] Desc
        var isToggled = e.Value;
        if (isToggled == true)
        {
            ((DetailViewModel)BindingContext).GetTaskTitleByDateDesc();
        }
        else
        {
            ((DetailViewModel)BindingContext).OnDisappearingKeepingHeadline();
            ((DetailViewModel)BindingContext).GetTaskTitle();
        }
    }

    // method is called when the user is searching for specific tasks via search bar
    void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        string searchText = searchBar.Text;
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            ((DetailViewModel)BindingContext).DisableSwitch();
            ((DetailViewModel)BindingContext).GetItemsTasksSearch(searchText);
            // SearchBar searchBar = (SearchBar)sender;
            // searchResults.ItemsSource = DataService.GetSearchResults(searchBar.Text);
        }
        else
        {
            ((DetailViewModel)BindingContext).EnableSwitch();
            ((DetailViewModel)BindingContext).OnDisappearingKeepingHeadline();
            ((DetailViewModel)BindingContext).GetTaskTitle();
        }
    }
    
    // method is called then the item has been dragged in order to disable the SwipeView for a better user experience
    private void collectionView_OnDragItem(object sender, DragItemEventArgs e)
    {
        ((DetailViewModel)BindingContext).DisableSwipeView();
    }
    
    // method is called before the drag-and-drop-operation is completed in order to get the new position of the item that has been dragged
    private void collectionView_DropItem(object sender, DropItemEventArgs e)
    {
        newPosition = e.DropItemHandle;
    }

    // method is called after the drag-and-drop-operation is completed in order to store the reordered positions of the items
    // whose position has been affected by the drag-and-drop-operation
    private void collectionView_CompleteItemDragDrop(object sender, CompleteItemDragDropEventArgs e)
    {
        if (newPosition != e.ItemHandle)
        {
            ((DetailViewModel)BindingContext).EditTaskPosition(e.ItemHandle, newPosition, int.Parse(IdList));
            ((DetailViewModel)BindingContext).EnableSwipeView();
        }
        ((DetailViewModel)BindingContext).EnableSwipeView();
    }
}