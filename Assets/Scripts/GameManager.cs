using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Tile[,] tiles;

    [HideInInspector] public Queue<Lumin> matchedQueue;

    [SerializeField] public int minimumMatch;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private GameObject emitterPrefab;

    [SerializeField] private int gridHeight;

    [SerializeField] private int gridWidth;

    [SerializeField] private int tileSize;

    [SerializeField] private bool placeEmitterOnLumin;

    [SerializeField] private CameraBehaviour cameraBehaviour;

    private List<GameObject> emitters;

    private float tileSpacing;

    public static GameManager instance = null;

    private bool validMovesLeft;

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
        matchedQueue = new Queue<Lumin>();
        validMovesLeft = false;
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
                Tile newTile = Instantiate(tilePrefab, position, Quaternion.identity).GetComponent<Tile>();
                newTile.x = x;
                newTile.y = y;
                tiles[y, x] = newTile;
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
        if (tile.lumin != null && !placeEmitterOnLumin)
        {
            return;
        }

        if (IsValidEmitterPlacement(tile, emitters[0].GetComponent<Emitter>()))
        {
            GameObject placedPiece = emitters[0];
            emitters.RemoveAt(0);
            placedPiece.transform.position = tile.transform.position;
            placedPiece.GetComponent<Emitter>().Placed(tile.y, tile.x);
            placedPiece.transform.localScale = Vector3.one;
            FillEmitters();
            cameraBehaviour.Nudge();

            PopLumins();
            CheckForValidMoves(emitters[0].GetComponent<Emitter>());
        }

    }

    private bool IsTileFree(int y, int x)
    {
        if (y < 0 || y > gridHeight - 1 || x < 0 || x > gridWidth - 1)
        {
            return false;
        }

        if (tiles[y, x].lumin != null)
        {
            return false;
        }

        return true;
    }

    private bool IsValidEmitterPlacement(Tile tile, Emitter emitter)
    {
        if (emitter.upColor != LuminColor.NONE && !IsTileFree(tile.y + 1, tile.x))
        {
            return false;
        }

        if (emitter.downColor != LuminColor.NONE && !IsTileFree(tile.y - 1, tile.x))
        {
            return false;
        }

        if (emitter.leftColor != LuminColor.NONE && !IsTileFree(tile.y, tile.x - 1))
        {
            return false;
        }

        if (emitter.rightColor != LuminColor.NONE && !IsTileFree(tile.y, tile.x + 1))
        {
            return false;
        }

        return true;
    }

    private void PopLumins()
    {
        while (matchedQueue.Count > 0)
        {
            Lumin lumin = matchedQueue.Dequeue();
            lumin.Pop();
        }
        matchedQueue.Clear();
    }

    private void CheckForValidMoves(Emitter emitter)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                validMovesLeft = IsValidEmitterPlacement(GameManager.instance.tiles[y, x], emitter);
                if (validMovesLeft) {
                    return;
                }
            }
        }
    }
}
