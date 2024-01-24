//<full>
namespace Vertexes.Views;

public partial class EdgeListPage : ContentPage
{
    public EdgeListPage()
    {
        InitializeComponent();
    }

    //<event>
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        edgeCollection.SelectedItem = null;
    }
    //</event>
}
//</full>
