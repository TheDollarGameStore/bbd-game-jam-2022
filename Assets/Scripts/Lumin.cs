using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LuminColor
{
    BLUE,
    GREEN,
    YELLOW,
    RED,
    NONE
}

public class Lumin : MonoBehaviour
{
    public LuminColor color;
    [HideInInspector] public int x;
    [HideInInspector] public int y;

    [SerializeField] private GameObject popEffectPrefab;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.color = Colors.GetColorFromEnum(color);
    }

    public void CheckNeighbours(Queue<Lumin> matchedQueue)
    {
        if (!matchedQueue.Contains(this))
        {
            matchedQueue.Enqueue(this);
        }

        Tile[,] tiles = GameManager.instance.tiles;
        if (y != 0)
        {
            CheckMatch(matchedQueue, tiles[y - 1, x].lumin);
        }
        if (x != 0)
        {
            CheckMatch(matchedQueue, tiles[y, x - 1].lumin);
        }
        if (y != tiles.GetLength(0) - 1)
        {
            CheckMatch(matchedQueue, tiles[y + 1, x].lumin);
        }
        if (x != tiles.GetLength(1) - 1)
        {
            CheckMatch(matchedQueue, tiles[y, x + 1].lumin);
        }
    }

    private void CheckMatch(Queue<Lumin> matchedQueue, Lumin lumin)
    {
        if (lumin != null && !matchedQueue.Contains(lumin) && lumin.color == color)
        {
            matchedQueue.Enqueue(lumin);
            lumin.CheckNeighbours(matchedQueue);
        }
    }

    public void Pop()
    {
        GameManager.instance.tiles[y, x].lumin = null;
        Instantiate(popEffectPrefab, GameManager.instance.tiles[y, x].transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
