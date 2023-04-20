using UnityEngine;

namespace Infrastructure
{
public class GameBootstraper : MonoBehaviour
{
    private Game _game;

    private void Awake()
    {
        _game = new Game();
        DontDestroyOnLoad(this);
    }
}
}