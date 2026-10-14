using ListenApp.ViewModel;
using DevExpress.Maui.CollectionView;

namespace ListenApp;

// following the MVVM principle the View's purpose is how to display information
public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm) // dependency injection to inject the MainViewModel in the MainPage
    {
        InitializeComponent(); // initializes the controls
        BindingContext = vm; // setting the BindingContext for the DetailPage to the ViewModel
    }

    // start with the default functionality of the OnAppearing-Method and then continue with the overridden part in order
    // to display all the items of the Lists-Database-Table
     protected override void OnAppearing()
     {
         base.OnAppearing();
         ((MainViewModel)BindingContext).OnAppearing();
     }
     
     // method is called then the item has been dragged in order to disable the SwipeView for a better user experience
     private void collectionView_OnDragItem(object sender, DragItemEventArgs e)
     {
         ((MainViewModel)BindingContext).DisableSwipeView();
     }
     
     private int newPosition;
     
     // method is called before the drag-and-drop-operation is completed in order to get the new position of the item that has been dragged
     private void collectionView_DropItem(object sender, DropItemEventArgs e)
     {
         newPosition = e.DropItemHandle;
     }
     
     // method is called when the drag-and-drop-operation is completed in order to store the item's new position in the database
     // and to enable the SwipeView that has been blocked during drag-and-drop-operations
     private void collectionView_CompleteItemDragDrop(object sender, CompleteItemDragDropEventArgs e)
     {
         if (newPosition != e.ItemHandle)
         {
             ((MainViewModel)BindingContext).EditListPosition(e.ItemHandle, newPosition);
             ((MainViewModel)BindingContext).EnableSwipeView();
         }
         ((MainViewModel)BindingContext).EnableSwipeView();
     }
     
     private async void Button_Clicked(object sender, EventArgs e)
     {
         await Navigation.PushModalAsync(new PromptPage(), animated: false);
     }
     
} 