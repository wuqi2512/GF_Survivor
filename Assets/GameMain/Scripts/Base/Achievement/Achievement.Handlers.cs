using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


public partial class AchievementComponent : GameFrameworkComponent
{
    private static Dictionary<int, Func<GameEventArgs, int>> s_Handlers;

    static AchievementComponent()
    {
        s_Handlers = new Dictionary<int, Func<GameEventArgs, int>>();
        s_Handlers.Add(typeof(HideEntityInLevelEventArgs).GetHashCode(), HideEntityInLevelHandler);
    }

    private static int HideEntityInLevelHandler(GameEventArgs e)
    {
        var ne = e as HideEntityInLevelEventArgs;

        if (ne.IsEnemy && ne.IsEnemyDead)
        {
            return 1;
        }

        return 0;
    }
}