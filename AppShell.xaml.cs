namespace Vertexes;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.VertexPage), typeof(Views.VertexPage));
        Routing.RegisterRoute(nameof(Views.EdgePage), typeof(Views.EdgePage));
        Routing.RegisterRoute(nameof(Views.VertexListPage), typeof(Views.VertexListPage));
        Routing.RegisterRoute(nameof(Views.ProcessListPage), typeof(Views.ProcessListPage));
        Routing.RegisterRoute(nameof(Views.SchedulerPage), typeof(Views.SchedulerPage));

    }
}