using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Vertexes.Models;

namespace Vertexes.ViewModels;

internal class EdgeListViewModel : IQueryAttributable
{


    public ObservableCollection<EdgeViewModel> AllEdges { get; private set; }
    public ICommand NewCommand { get; }
    public ICommand SelectEdgeCommand { get; }
    
    public EdgeListViewModel()
    {
        AllEdges = new ObservableCollection<EdgeViewModel>(Models.Edge.LoadAll(1).Select(n => new EdgeViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewEdgeAsync);
        SelectEdgeCommand = new AsyncRelayCommand<EdgeViewModel>(SelectEdgeAsync);
    }

    private async Task NewEdgeAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.EdgePage));
    }

    private async Task SelectEdgeAsync(EdgeViewModel edge)
    {
        if (edge != null)
            await Shell.Current.GoToAsync($"{nameof(Views.EdgePage)}?load={edge.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string edgeId = query["deleted"].ToString();
            EdgeViewModel matchedEdge = AllEdges.Where((n) => n.Identifier == edgeId).FirstOrDefault();

            // If vertex exists, delete it
            if (matchedEdge != null)
                AllEdges.Remove(matchedEdge);
        }
        else if (query.ContainsKey("saved"))
        {
            string edgeId = query["saved"].ToString();
           EdgeViewModel matchedEdge = AllEdges.Where((n) => n.Identifier == edgeId).FirstOrDefault();

            // If vertex is found, update it
            if (matchedEdge != null)
            {
                matchedEdge.Reload();
                AllEdges.Move(AllEdges.IndexOf(matchedEdge), 0);
            }
            // If vertex isn't found, it's new; add it.
            else
                AllEdges.Insert(0, new EdgeViewModel(Models.Edge.Load(edgeId)));
        }
    }
}