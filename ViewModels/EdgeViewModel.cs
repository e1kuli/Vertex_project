using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Vertexes.ViewModels;

internal class EdgeViewModel : ObservableObject, IQueryAttributable
{
    private Models.Edge _edge;

    public string NameParent
    {
        get => _edge.NameParent;
        set
        {
            if (_edge.NameParent != value)
            {
                _edge.NameParent = value;
                OnPropertyChanged();
            }
        }
    }
    public string NameChild
    {
        get => _edge.NameChild;
        set
        {
            if (_edge.NameChild != value)
            {
                _edge.NameChild = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _edge.Date;

    public int Id => _edge.Id;

    public string Identifier => _edge.Id.ToString();
    //public string Identifier => _vertex.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public EdgeViewModel()
    {
        _edge = new Models.Edge();
        SetupCommands();
    }
    
    public EdgeViewModel(Models.Edge vertex)
    {
        _edge = vertex;
        SetupCommands();
    }

    private void SetupCommands()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _edge.Date = DateTime.Now;
        _edge.Save();

        await Shell.Current.GoToAsync($"..?saved={_edge.Id}");
    }

    private async Task Delete()
    {
        _edge.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_edge.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _edge = Models.Edge.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _edge = Models.Edge.Load(_edge.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(NameParent));
        OnPropertyChanged(nameof(NameChild));
        OnPropertyChanged(nameof(Date));
    }
}