using System;
using System.Collections.Generic;
using System.Drawing;

public class TileMapLogic
{
    private TileMapData m_TileMapData;
    private List<Point> m_EmptyPosList;
    private Random m_Random;
    private int m_Score;
    private Direction m_Input;

    public event Action<Point, Point> OnMoveTile;
    public event Action<Point, Point, int> OnMergeTile;
    public event Action<Point, int> OnAddTile;
    public event Action<int> OnScoreChanged;

    public TileMapLogic(int height, int width)
    {
        m_TileMapData = new TileMapData(height, width);
        m_EmptyPosList = new List<Point>();
        long tick = DateTime.Now.Ticks;
        m_Random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
    }

    public void Start()
    {
        RandomAddTile();
    }

    public void Restart()
    {
        Reset();
        RandomAddTile();
    }

    public void Update()
    {
        if (m_Input == Direction.None)
        {
            return;
        }

        Move(m_Input);
        RandomAddTile();
        m_Input = Direction.None;
    }


    private void Move(Direction dir)
    {
        int i = 0;
        int j = 0;
        Point deltaI = default;
        Point deltaJ = default;
        Point cur = default;
        Point offset = default;

        if (dir == Direction.Up)
        {
            i = m_TileMapData.Height;
            j = m_TileMapData.Width;
            deltaI = new Point(0, 1);
            deltaJ = new Point(1, 0);
            cur = new Point(0, 0);
            offset = new Point(0, -1);
        }
        else if (dir == Direction.Right)
        {
            i = m_TileMapData.Width;
            j = m_TileMapData.Height;
            deltaI = new Point(-1, 0);
            deltaJ = new Point(0, 1);
            cur = new Point(m_TileMapData.Width - 1, 0);
            offset = new Point(1, 0);
        }
        else if (dir == Direction.Down)
        {
            i = m_TileMapData.Height;
            j = m_TileMapData.Width;
            deltaI = new Point(0, -1);
            deltaJ = new Point(-1, 0);
            cur = new Point(m_TileMapData.Width - 1, m_TileMapData.Height - 1);
            offset = new Point(0, 1);
        }
        else if (dir == Direction.Left)
        {
            i = m_TileMapData.Width;
            j = m_TileMapData.Height;
            deltaI = new Point(1, 0);
            deltaJ = new Point(0, -1);
            cur = new Point(0, m_TileMapData.Height - 1);
            offset = new Point(-1, 0);
        }

        int score = m_Score;
        for (int k = 0; k < i; k++)
        {
            Point temp = cur;
            for (int l = 0; l < j; l++)
            {
                PushTile(temp, offset, k);
                temp.Offset(deltaJ);
            }
            cur.Offset(deltaI);
        }

        if (score != m_Score)
        {
            OnScoreChanged?.Invoke(m_Score);
        }
    }

    private void PushTile(Point pos, Point offset, int distance)
    {
        if (!m_TileMapData.IsPosValid(pos))
        {
            throw new Exception(pos.ToString());
        }
        if (m_TileMapData.IsPosEmpty(pos))
        {
            return;
        }

        Point moveTo = pos;
        for (int i = 0; i < distance; i++)
        {
            Point temp = moveTo;
            temp.Offset(offset);
            if (!m_TileMapData.IsPosValid(temp))
            {
                if (!moveTo.Equals(pos))
                {
                    MoveTile(pos, moveTo);
                    return;
                }
            }

            if (m_TileMapData.IsPosEmpty(temp))
            {
                moveTo = temp;
                continue;
            }

            if (TryMergeTile(pos, temp))
            {
                return;
            }
            else
            {
                if (!moveTo.Equals(pos))
                {
                    MoveTile(pos, moveTo);
                    return;
                }
            }
        }

        if (!moveTo.Equals(pos))
        {
            MoveTile(pos, moveTo);
            return;
        }
    }

    private void MoveTile(Point a, Point b)
    {
        int value = m_TileMapData.GetValue(a);

        m_TileMapData.ClearValue(a);
        m_TileMapData.SetValue(b, value);
        OnMoveTile?.Invoke(a, b);
    }

    private bool TryMergeTile(Point a, Point b)
    {
        int aValue = m_TileMapData.GetValue(a);
        int bValue = m_TileMapData.GetValue(b);

        if (aValue == bValue)
        {
            int value = aValue + bValue;
            m_Score += value;
            m_TileMapData.ClearValue(a);
            m_TileMapData.SetValue(b, value);
            OnMergeTile?.Invoke(a, b, value);
            return true;
        }

        return false;
    }

    private void RandomAddTile()
    {
        m_EmptyPosList.Clear();
        m_TileMapData.GetAllEmptyTilePos(m_EmptyPosList);

        int randomValue = m_Random.Next(m_EmptyPosList.Count);
        Point pos = m_EmptyPosList[randomValue];
        m_TileMapData.SetValue(pos, 2);
        OnAddTile?.Invoke(pos, 2);
    }

    private void Reset()
    {
        m_TileMapData.Clear();
        long tick = DateTime.Now.Ticks;
        m_Random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        m_Score = 0;
    }

    public void SetInput(Direction direction)
    {
        m_Input = direction;
    }
}