using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Vertexes.Models;
using System.Collections.Generic;
using System.Collections;
using Vertexes.Views;
using System.ComponentModel;
//using Windows.Networking.Sockets;
//using CloudKit;

namespace Vertexes.ViewModels;

internal class VertexViewModel : ObservableObject, IQueryAttributable
{
    //модель для построения каждой ячейки
    private Models.Vertex _vertex;
    //все объекты системы
    public IEnumerable<Models.Vertex> AllVertexes { get; set; }
    //из них все вершины
    public List<string> VertexesForPicker { get; set; }
    //все ребра
    public IEnumerable<Models.Edge> AllEdges { get; set; }
    public List<string> EdgesForPicker { get; set; }
    public List<Models.Vertex> MyParents = new();
    public List<Models.Vertex> MyChildren = new();
    //public ICommand SelectVertexCommand { get; }
    public int MyEventId;
    public IEnumerable<Link> _fristLink { get; set; }
    public IEnumerable<Link> _secondLink { get; set; }

    public ObservableCollection<GroupLinksClass<string, AllLinksClass>> GroupLinks
    {
        get { return LinkGrabber(); }
        set
        {
            if (GroupLinks != value)
            {
                GroupLinks = value;
                OnPropertyChanged();
            }
        }

    }
    /*public List<AllLinksClass> AllLinks
    {
        get { return LinkGrabber(); }
        set
        {
            if (AllLinks != value)
            {
                AllLinks = value;
                OnPropertyChanged();
            }
        }
    }*/
    public string Name
    {
        get => _vertex.Name;
        set
        {
            if (_vertex.Name != value)
            {
                _vertex.Name = value;
                OnPropertyChanged();
            }
        }
    }
    public int Type 
    { 
        get =>_vertex.Type; 
        set => _vertex.Type = value; 
    }

    public string Text
    {
        get => _vertex.Text;
        set
        {
            if (_vertex.Text != value)
            {
                _vertex.Text = value;
                OnPropertyChanged();
            }
        }

    }
    public DateTime Date => _vertex.Date;

    public int Id => _vertex.Id;

    public string Identifier => _vertex.Id.ToString();

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand LinkCommand { get; private set; }
    public ICommand GoToVertexCommand { get; private set; }
    public ICommand SaveEventCommand { get; private set; }
    public ICommand DeleteLinkCommand {  get; private set; }
    public ICommand DeleteEventCommand { get; private set; }

    public VertexViewModel()
    {
        _vertex = new Models.Vertex();
        
        SetupCommands();
    }
    
    public VertexViewModel(Models.Vertex vertex)
    {
        _vertex = vertex;
        SetupCommands();

    }
    bool sValue = false;
    
    private void SetupCommands()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        GoToVertexCommand = new AsyncRelayCommand<Vertexes.Models.AllLinksClass>(GoToVertexAsync);
        LinkCommand = LinkEnabled();
        SaveEventCommand = SaveEvent();
        LinkGrabber();
        DeleteLinkCommand = DeleteLink();
        //DeleteEventCommand = DeleteEvent();
        AllVertexes = App.Database.GetAll<Vertex>();
        VertexesForPicker = AllVertexes.Select(n => n.Name).ToList();
        AllEdges = App.Database.GetAll<Edge>();
        EdgesForPicker = AllEdges.Select(n => n.NameParent).ToList();
        var _t = AllEdges.Select(n => n.NameChild);
        foreach(var t in _t)
        {
            EdgesForPicker.Add(t);
        }

    }

    private ICommand DeleteLink()
    {
        return new Command((obj) =>
        {
            if (obj != null)
            {
                GroupLinksClass<string, AllLinksClass> arr = (GroupLinksClass<string, AllLinksClass>)obj;
               
            }
        });
    }
    public void LinkEnable(object sender, EventArgs e)
    {
        LinkEnabled();
    }

    public ICommand SaveEvent()
    {

        return new Command((obj) =>
        {
            if (obj != null)
            {
                DateTime[] arr = ((IEnumerable<object>)obj).Cast<object>()
                                .Select(x=> DateTime.Parse(x.ToString())).ToArray();
                MyEvent myEv = new()
                {
                    VertexId = _vertex.Id,
                    From = arr[0],
                    To = arr[1],
                    EventName = _vertex.Name
                };
                if (MyEventId != 0)
                {
                    myEv.Id = MyEventId;
                }
                App.Database.SaveEvent(myEv);
                if (MyEventId == 0)
                {
                    MyEvent _tmpEvent = (MyEvent)App.Database.GetItem<MyEvent>(myEv.Id);
                    MyEventId = _tmpEvent.Id;
                }
            }
}
        );
    }
    public ICommand LinkEnabled()
    {

        return new Command(
                    execute: (obj) => //execute: (object? args) =>
                    {

                        if (obj != null)
                        {
                            string[] arr = ((IEnumerable<object>)obj).Cast<object>()
                                     .Select(x => x.ToString())
                                     .ToArray();
                            if (arr[0] != null && arr[1] != null)
                                sValue = true;

                            NewLink(arr);
                        }
                    }
               );// ,canExecute: (obj) => { return sValue; });
    }

    public ObservableCollection<GroupLinksClass<string, AllLinksClass>> LinkGrabber()
    {
        _fristLink = App.Database.GetAll<Link>().Where(l => l.FirstVertexId == _vertex.Id);
        _secondLink = App.Database.GetAll<Link>().Where(l => l.SecondVertexId == _vertex.Id);
        List<AllLinksClass> result = new();
        foreach (Link l in _fristLink )
        {
            Edge _t = (Edge)App.Database.GetItem<Edge>(l.EdgeId);
            Vertex _y = (Vertex)App.Database.GetItem<Vertex>(l.SecondVertexId);
            AllLinksClass res = new AllLinksClass(_t.NameParent, _y.Name, _y.Id);
            result.Add(res);
            if (l.SameGroupIndicator == 1)
                MyChildren.Add(_y);
        }
        foreach (Link l in _secondLink)
        {
            Edge _t = (Edge)App.Database.GetItem<Edge>(l.EdgeId);
            Vertex _y = (Vertex)App.Database.GetItem<Vertex>(l.FirstVertexId);
            AllLinksClass res = new AllLinksClass(_t.NameChild, _y.Name, _y.Id);
            result.Add(res);
            if (l.SameGroupIndicator == 1)
                MyParents.Add(_y);
        }
        var tmp = result.GroupBy(e => e.edge_name).Select(g => new GroupLinksClass<string, AllLinksClass>(g.Key, g));
        return new ObservableCollection<GroupLinksClass<string, AllLinksClass>>(tmp);

    }




private void NewLink(string[] edgeToVertex)
    {
        _vertex.Save();

        Vertex _vert = VertTransform(edgeToVertex[0]);
        Dictionary<int, Edge> _edgeWithWay = EdgeTransform(edgeToVertex[1]);
        Link link = new Link();

        if (_edgeWithWay.ContainsKey(0))
        {
            link.FirstVertexId = _vertex.Id; 
            link.EdgeId = _edgeWithWay[0].Id;
            link.SecondVertexId = _vert.Id;
        }
        if (_edgeWithWay.ContainsKey(1))
        {
            link.FirstVertexId = _vert.Id;
            link.EdgeId = _edgeWithWay[1].Id;
            link.SecondVertexId = _vertex.Id; 
        }
        if (_vert.Type == _vertex.Type)
        {
            link.SameGroupIndicator = 1;
        }
        else
            link.SameGroupIndicator = 0;
        App.Database.SaveLink(link);
        RefreshProperties();
    }

    private Vertex VertTransform (object vertName)
    {

        return AllVertexes.Where(n =>  n.Name == vertName.ToString()).FirstOrDefault();
    }

    private Dictionary<int, Edge> EdgeTransform (string edgeName)
    {
        Edge _t = AllEdges.Where(n => n.NameParent == edgeName).FirstOrDefault();
        if (_t != null)
            return new Dictionary<int, Edge>() { { 0, _t } };
        else
        {
            _t = AllEdges.Where(n => n.NameChild == edgeName).FirstOrDefault();
            return new Dictionary<int, Edge>() { { 1, _t } };

        }

    }

    private async Task Save()
    {
        _vertex.Date = DateTime.Now;
        _vertex.Save();

        await Shell.Current.GoToAsync($"..?saved={_vertex.Id}");
    }

    private async Task Delete()
    {
        foreach (Link l in _fristLink)
            App.Database.DeleteItem<Link>(l.Id);
        foreach (Link l in _secondLink)
            App.Database.DeleteItem<Link>(l.Id);

        _vertex.Delete();

        await Shell.Current.GoToAsync($"..?deleted={_vertex.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _vertex = Models.Vertex.Load(query["load"].ToString());
            //AllLinks = LinkGrabber();
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _vertex = Models.Vertex.Load(_vertex.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Type));

        OnPropertyChanged(nameof(Date));
        OnPropertyChanged(nameof(GroupLinks));
        //OnPropertyChanged(nameof((LinkCommand as Command).ChangeCanExecute()));
        OnPropertyChanged(nameof(LinkCommand));


    }
    private async Task GoToVertexAsync(Vertexes.Models.AllLinksClass vertex)//GroupLinksClass<string, AllLinksClass> vertex)
    {
        if (vertex != null) { }
            await Shell.Current.GoToAsync($"{nameof(Views.VertexPage)}?load={vertex.vertex_id.ToString()}");
    }

}