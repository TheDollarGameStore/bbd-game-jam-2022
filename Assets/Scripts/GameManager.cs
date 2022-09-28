using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Tile[,] tiles;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private int gridHeight;

    [SerializeField] private int gridWidth;

    [SerializeField] private int tileSize;

    private float tileSpacing;

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        tileSpacing = tileSize / 8f;
        GenerateGrid();
    }

    // Update is called once per frame
    void GenerateGrid()
    {
        float totalSize = tileSize + tileSpacing;

        tiles = new Tile[gridHeight, gridWidth];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 position = new Vector2(-(totalSize * 1.5f) - ((gridWidth / 2f) * totalSize) + (x * totalSize), -((gridHeight / 2f) * totalSize) + (y * totalSize));
                tiles[y, x] = Instantiate(tilePrefab, position, Quaternion.identity).GetComponent<Tile>();
            }
        }
    }
}
