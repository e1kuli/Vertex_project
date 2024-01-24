using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Xml.Linq;
using Vertexes.Models;

namespace Vertexes.ViewModels;

internal class VertexListViewModel : ObservableObject, IQueryAttributable
{
    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string vertexId = query["deleted"].ToString();
            VertexViewModel matchedVertex = AllVertexes.Where((n) => n.Identifier == vertexId).FirstOrDefault();

            // If vertex exists, delete it
            if (matchedVertex != null)
                AllVertexes.Remove(matchedVertex);
        }
        else if (query.ContainsKey("saved"))
        {
            string vertexId = query["saved"].ToString();
            VertexViewModel matchedVertex = AllVertexes.Where((n) => n.Identifier == vertexId).FirstOrDefault();

            // If vertex is found, update it
            if (matchedVertex != null)
            {
                matchedVertex.Reload();
                AllVertexes.Move(AllVertexes.IndexOf(matchedVertex), 0);
            }
            // If vertex isn't found, it's new; add it.
            else
                AllVertexes.Insert(0, new VertexViewModel(Models.Vertex.Load(vertexId)));
        }
        else if (query.ContainsKey("loadList"))
        {
            IsMainPage = false;
            NewParentId = Int32.Parse(query["loadList"].ToString());
            NewParentName=(Vertex.Load(NewParentId).Name);
        }
        OnPageStart();

        RefreshProperties();

        //StartInput(0);
        //StartInput(1);
    }


    //Перечень всех записей из БД
    public ObservableCollection<VertexViewModel> AllVertexes { get; private set; }//добавить inotify

    //актуальные записи для текущей страницы
    public ObservableCollection<VertexViewModel> CurrentPageVertexList { get; private set; }
    public ObservableCollection<VertexViewModel> CurrentPageProcessList { get; private set; }

    //выводимые записи
    public ObservableCollection<VertexViewModel> SearchVertexes { get; private set; }
    public ObservableCollection<VertexViewModel> SearchProceses { get; private set; }

    //переманная определяет эта страница открыта впервые при запуске приложения или был переход в группу
    public bool IsMainPage = true;
    //Новый родитель, который задает переменную AllWertexes, что бы она состояла только из его наследников
    public int NewParentId=-1;
    public string NewParentName { get; private set; }

    public ICommand NewCommand { get; private set; }
    public ICommand SelectVertexCommand { get; private set; }
    public ICommand SelectProcessCommand { get; private set; }
    public ICommand BtnChangeClick { get; private set; }
    public ICommand VertexSearch { get; set; }
    public ICommand ProcessSearch { get; set; }
    //public string searchBox { get; set; }
    
    public VertexListViewModel()
    {
        NewParentName = "Главная страница";

        OnPageStart();

        //StartInput();
    }

    //
    /*public VertexListViewModel(ObservableCollection<VertexViewModel> allVertexes)
    {
        AllVertexes = allVertexes;
        OnPageStart();
    }*/

    public void OnPageStart()
    {
        //StartInput(0);
        //StartInput(1);
        //если страница запускается впервые - необходимо сформировать список только из верхушек

        if (IsMainPage)
        {
            AllVertexes = new ObservableCollection<VertexViewModel>(Models.Vertex.LoadAll().Select(n => new VertexViewModel(n)));//.Where(v => v.MyParents == null));
            var t = new ObservableCollection<VertexViewModel>(AllVertexes);
            foreach (var v in t)
            {
                if (v.MyParents.Count != 0) AllVertexes.Remove(v);
            }
        }
        else
        {
            VertexViewModel _tmp = new VertexViewModel(Models.Vertex.Load(NewParentId));
            AllVertexes = new ObservableCollection<VertexViewModel>(_tmp.MyChildren.Select(n => new VertexViewModel(n)));
            //NewParentName = AllVertexes.Where(n => n.Id == NewParentId).First().Name;
            //NewParentName = _tmp.Name;
            //OnPropertyChanged(nameof(NewParentName));
            //int a = 0;
        }

        CurrentPageVertexList = new ObservableCollection<VertexViewModel>(AllVertexes.Where(n => n.Type == 0));
        if (CurrentPageVertexList != null)
        {
            SearchVertexes = new ObservableCollection<VertexViewModel>(CurrentPageVertexList);
        }
        CurrentPageProcessList = new ObservableCollection<VertexViewModel>(AllVertexes.Where(n => n.Type == 1));
        if (CurrentPageProcessList != null)
        {
            SearchProceses = new ObservableCollection<VertexViewModel>(CurrentPageProcessList);
        }
        RefreshProperties();
        

        NewCommand = new AsyncRelayCommand(NewVertexAsync);
        SelectVertexCommand = new AsyncRelayCommand<VertexViewModel>(SelectVertexAsync);
        SelectProcessCommand = new AsyncRelayCommand<VertexViewModel>(SelectProcessAsync);
        BtnChangeClick = new AsyncRelayCommand(ChangeVertexAsync);
        VertexSearch = new Command(obj =>
        {
            IEnumerable<VertexViewModel> alvert = CurrentPageVertexList;
            alvert = alvert.Where(n => n.Name.Contains(obj.ToString(), StringComparison.OrdinalIgnoreCase));

            if (alvert != null)
            {
                SearchVertexes = new ObservableCollection<VertexViewModel>(alvert);
                RefreshProperties();
            }
        });
        ProcessSearch = new Command(obj =>
        {
            IEnumerable<VertexViewModel> alvert = CurrentPageProcessList;
            alvert = alvert.Where(n => n.Name.Contains(obj.ToString(), StringComparison.OrdinalIgnoreCase));
            if (alvert != null)
            {
                SearchProceses = new ObservableCollection<VertexViewModel>(alvert);
                RefreshProperties();
            }
        });

    }
    protected async Task NewVertexAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(Views.VertexPage)}?type={0}");
    }
    protected async Task NewProcessAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(Views.VertexPage)}?type={1}");
    }

    protected async Task SelectVertexAsync(VertexViewModel vertex)
    {
        if (vertex != null)
            await Shell.Current.GoToAsync($"{nameof(Views.VertexListPage)}?loadList={vertex.Id}");
    }
    protected async Task SelectProcessAsync(VertexViewModel vertex)
    {
        if (vertex != null)
            await Shell.Current.GoToAsync($"{nameof(Views.ProcessListPage)}?loadList={vertex.Id}");
    }

    protected async Task ChangeVertexAsync()
    {
        if (NewParentId >= 0)
            await Shell.Current.GoToAsync($"{nameof(Views.VertexPage)}?load={NewParentId}");

    }
    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(NewParentName));
        OnPropertyChanged(nameof(SearchProceses));
        OnPropertyChanged(nameof(SearchVertexes));
        OnPropertyChanged(nameof(CurrentPageProcessList));
        OnPropertyChanged(nameof(CurrentPageVertexList));


    }
}