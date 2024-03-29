using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Tile[,] tiles;

    [HideInInspector] public Queue<Lumin> matchedQueue;

    [SerializeField] public int minimumMatch;

    [HideInInspector] public Connector[,] verticalConnectors;

    [HideInInspector] public Connector[,] horizontalConnectors;

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private GameObject emitterPrefab;

    [SerializeField] private GameObject connectorPrefab;

    [SerializeField] private int gridHeight;

    [SerializeField] private int gridWidth;

    [SerializeField] private int tileSize;

    [SerializeField] private bool placeEmitterOnLumin;

    public CameraBehaviour cameraBehaviour;

    [SerializeField] private AudioClip popSound;

    [SerializeField] private Text scoreText;

    [SerializeField] private GameObject explosionGameOverPrefab;

    [SerializeField] private GameObject explosionThingy;

    [SerializeField] private AudioClip gameOverSound;

    [SerializeField] private GameObject successCanvas;

    private int score;

    private int displayScore;

    private List<GameObject> emitters;

    private float tileSpacing;

    public static GameManager instance = null;

    private bool validMovesLeft;

    private bool introPlaying = true;

    [HideInInspector] public int levelUnlocked;

    private readonly int[] scoreNeededToClearLevel = { 5000, 10000, 15000, 20000, 25000, 1000000 };

    public StarsManager starsManager;

    [SerializeField] private Text levelText;


    [SerializeField] private GameObject gameOverObject;

    private bool gameOver = false;

    private string gameMode;

    [HideInInspector] public bool hasPaused;

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
        gameMode = PlayerPrefs.GetString("GameMode", "");
        levelUnlocked = 1;
        SoundManager.Instance.levelUnlocked = levelUnlocked;
        introPlaying = true;
        emitters = new List<GameObject>();
        tileSpacing = tileSize / 8f;
        GenerateGrid();
        Invoke("FillEmitters", 2.675f);
        matchedQueue = new Queue<Lumin>();
        GenerateConnectorGrid();
        validMovesLeft = false;

        if (gameMode == "Endless")
        {
            Destroy(levelText.gameObject);
            scoreText.rectTransform.anchoredPosition = new Vector2(0f, -48f);
            scoreText.alignment = TextAnchor.MiddleCenter;
        }
    }

    void GenerateGrid()
    {
        int tilesGenerated = 0;
        float totalSize = tileSize + tileSpacing;

        tiles = new Tile[gridHeight, gridWidth];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 position = new Vector2(-((gridWidth / 2f) * totalSize) + (x * totalSize), -((gridHeight / 2f) * totalSize) + (y * totalSize));
                Tile newTile = Instantiate(tilePrefab, position, Quaternion.identity).GetComponent<Tile>();
                newTile.Invoke("Deploy", tilesGenerated / 100f);
                newTile.x = x;
                newTile.y = y;
                tiles[y, x] = newTile;
                tilesGenerated++;
            }
        }
    }

    void GenerateConnectorGrid()
    {
        float totalSize = tileSize + tileSpacing;
        verticalConnectors = new Connector[gridHeight - 1, gridWidth];
        for (int y = 0; y < gridHeight - 1; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 position = new Vector2(-((gridWidth / 2f) * totalSize) + (x * totalSize), -((gridHeight / 2f) * totalSize) + (y * totalSize) + (totalSize / 2f));
                Connector newConnector = Instantiate(connectorPrefab, position, Quaternion.identity).GetComponent<Connector>();
                newConnector.transform.Rotate(new Vector3(0, 0, 90));
                newConnector.tiles.Add(tiles[y, x]);
                newConnector.tiles.Add(tiles[y + 1, x]);
                tiles[y, x].connectors.Add(newConnector);
                tiles[y + 1, x].connectors.Add(newConnector);
                verticalConnectors[y, x] = newConnector;
            }
        }

        horizontalConnectors = new Connector[gridHeight, gridWidth - 1];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth - 1; x++)
            {
                Vector2 position = new Vector2(-((gridWidth / 2f) * totalSize) + (x * totalSize) + (totalSize / 2f), -((gridHeight / 2f) * totalSize) + (y * totalSize));
                Connector newConnector = Instantiate(connectorPrefab, position, Quaternion.identity).GetComponent<Connector>();
                newConnector.tiles.Add(tiles[y, x]);
                newConnector.tiles.Add(tiles[y, x + 1]);
                tiles[y, x].connectors.Add(newConnector);
                tiles[y, x + 1].connectors.Add(newConnector);
                horizontalConnectors[y, x] = newConnector;
            }
        }
    }

    void FillEmitters()
    {
        while (emitters.Count < 5)
        {
            emitters.Add(Instantiate(emitterPrefab, new Vector2(800f, 1200f), Quaternion.identity));
        }

        introPlaying = false;
    }

    private void Update()
    {
        if (introPlaying)
        {
            return;
        }

        if (gameOver)
        {
            return;
        }

        ShiftEmitters();

        if (Input.GetMouseButtonDown(0) && !hasPaused)
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

        if (Input.GetMouseButtonUp(0))
        {
            hasPaused = false;
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
            CheckForScoreReached();
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
        if (matchedQueue.Count != 0)
        {
            SoundManager.Instance.PlayPitched(popSound, Mathf.Min(0.5f + (matchedQueue.Count * 0.1f), 1.5f));
            cameraBehaviour.Shake(10f + matchedQueue.Count);
            score += (int)Math.Pow(matchedQueue.Count - 2, 2) * 100 + 100;
            while (matchedQueue.Count > 0)
            {
                Lumin lumin = matchedQueue.Dequeue();
                lumin.Pop();
            }
            matchedQueue.Clear();
        }
    }

    private void CheckForValidMoves(Emitter emitter)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                validMovesLeft = (tiles[y, x].lumin == null || placeEmitterOnLumin) && IsValidEmitterPlacement(tiles[y, x], emitter);
                if (validMovesLeft) {
                    //Debug.Log("Valid placement for emitter: " + emitter.upColor + ", " + emitter.leftColor + ", " + emitter.downColor + ", " + emitter.rightColor + ": X-" + x + ", Y-" + y);
                    return;
                }
            }
        }

        //If it reaches here, it means game over
        Debug.Log("You Lose");
        TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        gameOver = true;
        Invoke("GameOverEffects", 1.5f);
        SoundManager.Instance.PlayPitched(gameOverSound, 1f);
    }

    private void GameOverEffects()
    {
        DestroyGameboard();

        starsManager.SlowTime();
        explosionThingy.SetActive(true);
        explosionThingy.GetComponent<Transition>().alpha = 1f;
        explosionThingy.GetComponent<Transition>().ReplaySound();
        Invoke("ShowGameOverScreen", 2.5f);
    }

    private void DestroyGameboard()
    {
        foreach (GameObject em in emitters)
        {
            Destroy(em);
        }
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (tiles[y, x].lumin != null)
                {
                    Destroy(tiles[y, x].lumin.gameObject);
                }
                Destroy(tiles[y, x].gameObject);
            }
        }
        for (int y = 0; y < gridHeight - 1; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Destroy(verticalConnectors[y, x].gameObject);
            }
        }
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth - 1; x++)
            {
                Destroy(horizontalConnectors[y, x].gameObject);
            }
        }
    }

    private void ShakeGameOverScreen()
    {
        cameraBehaviour.Shake(60f);
    }

    private void ShowGameOverScreen()
    {
        gameOverObject.GetComponent<GameOverUI>().Reset();
        gameOverObject.SetActive(true);
    }

    private void CheckForScoreReached()
    {

        if (score >= scoreNeededToClearLevel[levelUnlocked - 1])
        {
            levelUnlocked++;
            SoundManager.Instance.levelUnlocked = levelUnlocked;


            if (gameMode == "Endless")
            {
                return;
            }

            displayScore = 0;
            starsManager.InitiateZoomies();
            score = 0;
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (tiles[y, x].lumin != null)
                    {
                        tiles[y, x].lumin.Pop();
                    }
                }
            }

            if (levelUnlocked >= 6)
            {
                starsManager.SlowTime();
                DestroyGameboard();
                successCanvas.SetActive(true);
                gameOver = true;
            }
        }
    }

    public void RestartLevel()
    {
        displayScore = 0;
        score = 0;
        starsManager.InitiateZoomies();
        gameOverObject.GetComponent<GameOverUI>().Reset();
        gameOverObject.SetActive(false);
        introPlaying = true;
        emitters = new List<GameObject>();
        tileSpacing = tileSize / 8f;
        GenerateGrid();
        Invoke("FillEmitters", 2.675f);
        matchedQueue = new Queue<Lumin>();
        GenerateConnectorGrid();
        validMovesLeft = false;
        gameOver = false;
    }

    private void FixedUpdate()
    {
        if (displayScore != score)
        {
            if (score - displayScore > 500)
            {
                displayScore += 100;
            }
            else if (score - displayScore > 50)
            {
                displayScore += 10;
            }
            else
            {
                displayScore += 1;
            }
        }

        if (gameMode == "Endless")
        {
            scoreText.text = displayScore.ToString();
        }
        else
        {
            
            if (levelUnlocked < 7)
            {
                levelText.text = "LEVEL " + levelUnlocked.ToString();
                scoreText.text = displayScore.ToString() + " / " + scoreNeededToClearLevel[levelUnlocked - 1].ToString();
            }
            else
            {
                levelText.text = ":)";
                scoreText.text = ":)";
            }
        }
    }
}
