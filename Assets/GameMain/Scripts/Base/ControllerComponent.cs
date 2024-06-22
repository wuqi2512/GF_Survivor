using UnityGameFramework.Runtime;

public class ControllerComponent : GameFrameworkComponent
{
    public Player Player { get; private set; }

    public void SetPlayer(Player player)
    {
        Player = player;
    }
}