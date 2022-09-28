using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Tile[,] tiles;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private GameObject emitterPrefab;

    [SerializeField] private int gridHeight;

    [SerializeField] private int gridWidth;

    [SerializeField] private int tileSize;

    private List<GameObject> emitters;

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
        emitters = new List<GameObject>();
        tileSpacing = tileSize / 8f;
        GenerateGrid();
        FillEmitters();
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
                Vector2 position = new Vector2(-((gridWidth / 2f) * totalSize) + (x * totalSize), -((gridHeight / 2f) * totalSize) + (y * totalSize));
                tiles[y, x] = Instantiate(tilePrefab, position, Quaternion.identity).GetComponent<Tile>();
            }
        }
    }

    void FillEmitters()
    {
        while (emitters.Count < 5)
        {
            emitters.Add(Instantiate(emitterPrefab,  new Vector2(800f, 1200f), Quaternion.identity));
        }
    }

    private void Update()
    {
        ShiftEmitters();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    PlaceEmitter(hit.collider.gameObject.GetComponent<Tile>());
                }
            }
        }
    }

    void ShiftEmitters()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].transform.position = Vector3.Lerp(emitters[i].transform.position, new Vector2(800f, -600f + (i * 275f)), 10f * Time.deltaTime);
        }
    }

    void PlaceEmitter(Tile tile)
    {
        if (tile.lumin != null)
        {
            return;
        }

        GameObject placedPiece = emitters[0];
        emitters.RemoveAt(0);
        placedPiece.transform.position = tile.transform.position;
        placedPiece.transform.localScale = Vector3.one;
        FillEmitters();
    }
}
