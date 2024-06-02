using System.Drawing;
using UnityEngine;

public class TileMapController
{
    public TileMapForm view;
    public TileMapLogic logic;

    public TileMapController(TileMapForm view, TileMapLogic logic)
    {
        this.view = view;
        this.logic = logic;

        logic.OnAddTile += OnAddTile;
        logic.OnMergeTile += OnMergeTile;
        logic.OnMoveTile += OnMoveTile;
        logic.OnScoreChanged += OnScoreChanged;
    }

    private void OnAddTile(Point pos, int value)
    {
        view.AddBox(PointToVector2Int(pos), value);
    }

    private void OnMoveTile(Point pos, Point toPos)
    {
        view.MoveTile(PointToVector2Int(pos), PointToVector2Int(toPos), -2);
    }

    private void OnMergeTile(Point pos, Point toPos, int value)
    {
        view.MoveTile(PointToVector2Int(pos), PointToVector2Int(toPos), value);
    }

    private void OnScoreChanged(int score)
    {
        view.UpdateScore(score);
    }

    public void Restart()
    {
        view.RestartGame();
        logic.Restart();
    }

    private Vector2Int PointToVector2Int(Point pos)
    {
        return new Vector2Int(pos.X, pos.Y);
    }
}