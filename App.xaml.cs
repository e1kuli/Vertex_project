using Vertexes.Models;

namespace Vertexes;

public partial class App : Application
{
    public const string DATABASE_NAME = "Notes.db";
    public static VertexRepository database;
    public static VertexRepository Database
    {
        get
        {
            if (database == null)
            {
                database = new VertexRepository(
                    Path.Combine("C:\\Users\\e1_Kuli\\source\\repos", DATABASE_NAME));
                // Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));


            }
            return database;
        }
    }
    public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
	}
}
