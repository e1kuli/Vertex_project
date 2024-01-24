//<full>
using Vertexes.ViewModels;
using Microsoft.Maui.Controls;

namespace Vertexes.Views;

public partial class ProcessListPage : ContentPage
{
	public ProcessListPage()
	{
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
    protected override void OnDisappearing() { base.OnDisappearing(); }
    //<event>
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        processCollection.SelectedItem = null;
    }
}