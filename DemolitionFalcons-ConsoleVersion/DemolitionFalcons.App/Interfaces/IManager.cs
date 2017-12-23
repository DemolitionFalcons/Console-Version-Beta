namespace DemolitionFalcons.App.Interfaces
{
    using System.Collections.Generic;

    public interface IManager
    {
        string Quit();

        string CreateCharacter(IList<string> arguments);

        string InspectCharacter(IList<string> arguments);

        string DeleteCharacter(IList<string> arguments);

        string AddRoom(IList<string> arguments);

        string JoinRoom(IList<string> arguments);

        string RegisterUser();

        string Help();

        string ResetDatabase();

        string StartGame();
    }
}
