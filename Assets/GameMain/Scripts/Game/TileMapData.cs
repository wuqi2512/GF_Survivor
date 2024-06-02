using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[Serializable]
public class TileMapData
{
    private int m_Height;
    private int m_Width;
    private int[,] m_Values;

    public TileMapData(int height, int width)
    {
        m_Height = height;
        m_Width = width;
        m_Values = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                m_Values[i, j] = -1;
            }
        }
    }

    public int Height => m_Height;
    public int Width => m_Width;

    public int GetValue(Point pos)
    {
        return m_Values[pos.Y, pos.X];
    }

    public void SetValue(Point pos, int value)
    {
        if (value == -1)
        {
            throw new Exception("Try to set to -1.");
        }

        m_Values[pos.Y, pos.X] = value;
    }

    public void ClearValue(Point pos)
    {
        m_Values[pos.Y, pos.X] = -1;
    }

    public bool IsPosEmpty(Point pos)
    {
        return m_Values[pos.Y, pos.X] == -1;
    }

    public bool IsFull()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (m_Values[i, j] == -1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsPosValid(Point pos)
    {
        if (pos.X < 0 || pos.X >= m_Width
            || pos.Y < 0 || pos.Y >= m_Height)
        {
            return false;
        }

        return true;
    }

    public void GetAllEmptyTilePos(List<Point> list)
    {
        list.Clear();

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (m_Values[i, j] == -1)
                {
                    list.Add(new Point(j, i));
                }
            }
        }
    }

    public int[,] GetValueArray()
    {
        return m_Values;
    }

    public void Clear()
    {
        for (int i = 0; i < m_Height; i++)
        {
            for (int j = 0; j < m_Width; j++)
            {
                m_Values[i, j] = -1;
            }
        }
    }
}