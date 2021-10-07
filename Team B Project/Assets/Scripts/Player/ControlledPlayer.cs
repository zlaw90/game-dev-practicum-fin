using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class ControlledPlayer : IPlayer
{
    public static ControlledPlayer Instance;
    public event Action<bool> GameDidEnd;
    public void GameEnd(bool win)
    {
   //     Debug.Log($"Game Over: {(win ? "Win!" : "Lose")}");
        GameDidEnd?.Invoke(win);
    }
    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    public override List<Planet> OwnedPlanets()
    {
        return _planets.Where(x => x.control == Planet.controlEnum.player1).ToList();
    }
}
