using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    NavMeshSurface navMesh;
    public Vector2 size;
    public GameObject sampleHex;
    GameObject[,] grid;

    public bool gameIsOn = false;

    GameObject holderGrid;
    public PlayerStats playerStats;

    public float nextBake = 0;
    int toBake = 0;

    public CellSelector cellSel;
    public EnemyManager enemyMan;
    public int timer = 0;
    public Text gameLoopText;

    [SerializeField]
    public Waves[] waves;
    int currentWave = 0;

    public GameObject tuto;

    [System.Serializable]
    public struct Waves
    {
        public int playerConstructionTime;
        public int holeQuantity;
        public int howManyTimes;
        public int timeBetweenSpawn;
        public int howManyEnemies;
    }
    public GameObject GUI1,GUI2,GUI3,GUIBUTTON,GUIPANEL,GUITUTO;
    public GameObject loseGUI,winGUI;
    void Start()
    {
        loseGUI.SetActive(false);winGUI.SetActive(false);
        loseGUI.transform.position = loseGUI.transform.position + new Vector3(1000,0,0);
        winGUI.transform.position = winGUI.transform.position + new Vector3(1000,0,0);
        navMesh = this.GetComponent<NavMeshSurface>();
        StartCoroutine(Restart());
    }


    IEnumerator Restart()
    {
        gameIsOn = false;
        //Clear all the tiles
        if (holderGrid != null)
            Destroy(holderGrid);
        holderGrid = new GameObject("Grid Holder");
        holderGrid.transform.SetParent(transform);
        //Generate the grid
        grid = new GameObject[(int)size.x, (int)size.y];

        GUI1.SetActive(false);
        GUI2.SetActive(false);
        GUI3.SetActive(false);
        GUIPANEL.SetActive(false);
        GUIBUTTON.SetActive(false);
        tuto.SetActive(true);
        GUITUTO.SetActive(true);

        while (tuto.GetComponent<TutorialManager>().linhaTexto < 3)
        {
            yield return null;
        }
        GUI2.SetActive(true);

         while (tuto.GetComponent<TutorialManager>().linhaTexto < 5)
        {
            yield return null;
        }
        GUIPANEL.SetActive(true);
        while (tuto.GetComponent<TutorialManager>().linhaTexto < 6)
        {
            yield return null;
        }
        GenerateGrid();
        while (tuto.GetComponent<TutorialManager>().linhaTexto < 7)
        {
            yield return null;
        }
        SpawnInitialColony();
        if(tuto.GetComponent<TutorialManager>().linhaTexto < 21){
              GUITUTO.SetActive(false);
             yield return new WaitForSeconds(4);
              GUITUTO.SetActive(true);
        }

        while (tuto.GetComponent<TutorialManager>().linhaTexto < 10)
        {
            yield return null;
        }
        GUIBUTTON.GetComponent<Button>().interactable =false;
        GUIBUTTON.SetActive(true);

        while (tuto.GetComponent<TutorialManager>().linhaTexto < 21)
        {
            yield return null;
        }

        SetPlayer();
        StartCoroutine(GameLoop());

        GUI1.SetActive(true);
        GUI2.SetActive(true);
        GUI3.SetActive(true);
        GUIPANEL.SetActive(true);
        GUIBUTTON.SetActive(true);
        GUITUTO.SetActive(false);
        tuto.SetActive(false);
        GUIBUTTON.GetComponent<Button>().interactable =true;
        

        yield break;
    }

    IEnumerator GameLoop()
    {
       

        gameIsOn = true;
        // Disable cell editing for now
        cellSel.isOn = false;
        cellSel.TurnOffUI();
        while (gameIsOn)
        {
            if (currentWave >= waves.Length)
            {
                StartCoroutine(WinGame());
                break;
            }

            FindObjectOfType<SoundManager>().PlayTrack("Soundtrack");

            gameLoopText.text = "Inimigos irão invadir a colméia pelos buracos ao redor. Prepare-se!";
            enemyMan.ActivateHoles(waves[currentWave].holeQuantity);
            //REVEAL NEXT HOLES
            yield return new WaitForSeconds(3);

            //Give the player 10 seconds to build base
            timer = waves[currentWave].playerConstructionTime;
            gameLoopText.text = "Você tem " + timer + " segundos para organizar sua colméia.";
            cellSel.isOn = true;
            //LET PLAYER BUILD BASE


            //Wait for player input
            while (!Input.anyKeyDown)
                yield return null;
            //Begin countdown
            while (timer > 0)
            {
                yield return new WaitForSeconds(1);
                timer--;
                gameLoopText.text = "Você tem " + timer + " segundos para organizar sua colméia.";
                yield return null;
            }
            //TIME IS OUT


            //Turn off the cell GUI
            cellSel.isOn = false;
            cellSel.TurnOffUI();

            //RELEASE THE ENEMIES
            gameLoopText.text = "Sua colméia está em perigo!";
            enemyMan.isDone = false;
            StartCoroutine(enemyMan.ReleaseEnemies(waves[currentWave].howManyTimes, waves[currentWave].howManyEnemies, waves[currentWave].timeBetweenSpawn));

            while (!enemyMan.isDone){
                yield return null;
                if(playerStats.stats.royalCellCount <= 0 && playerStats.stats.royalJelly <= 0) {
                    StartCoroutine(LoseGame());
                    break;
                }
            }
            CheckNextBake();
            ReplenishCellHP();
            currentWave++;
            yield return null;
        }
        yield break;
    }

    void ReplenishCellHP()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                grid[x, y].GetComponent<Cell>().hp = grid[x, y].GetComponent<Cell>().maxHp;

            }
        }
    }
    void SpawnInitialColony()
    {
        //Spawn at middle by default
        Vector2 initialpoint = new Vector2((size.x / 2)+Random.Range(-2,1), (size.y / 2)+Random.Range(2,4));
        //Test for invalid cell
        //initialpoint = new Vector3(111,111);

        CellSelector cellSel = FindObjectOfType<CellSelector>();
        Vector2 itinerator;

        //Spawn initialTile (Royal Jelly)
        itinerator = initialpoint;
        TransformTile(itinerator, "potJelly");

        //Spawn pot honey
        itinerator = initialpoint + new Vector2(1, 1);
        TransformTile(itinerator, "potHoney");

        //Spawn pot propolis
        itinerator = initialpoint + new Vector2(0, 1);
        TransformTile(itinerator, "potPropolis");

        WallCells();

        Rebake();



    }

    void SetPlayer()
    {
        playerStats.stats.royalJelly = 20;
        playerStats.stats.polen = 30;
        playerStats.stats.honey = 20;
        playerStats.stats.propolis = 10;
        playerStats.stats.royalCellCount = 1;
    }

    void WallCells()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (grid[x, y].GetComponent<Cell>().ofType == "empty")
                {

                    if (CheckIfCellExists(x + 1, y))
                        if (grid[x + 1, y].GetComponent<Cell>().ofType != "empty" && grid[x + 1, y].GetComponent<Cell>().ofType != "goop")
                            TransformTile(new Vector2(x, y), "goop");
                    if (CheckIfCellExists(x - 1, y))
                        if (grid[x - 1, y].GetComponent<Cell>().ofType != "empty" && grid[x - 1, y].GetComponent<Cell>().ofType != "goop")
                            TransformTile(new Vector2(x, y), "goop");
                    if (CheckIfCellExists(x, y + 1))
                        if (grid[x, y + 1].GetComponent<Cell>().ofType != "empty" && grid[x, y + 1].GetComponent<Cell>().ofType != "goop")
                            TransformTile(new Vector2(x, y), "goop");
                    if (CheckIfCellExists(x, y - 1))
                        if (grid[x, y - 1].GetComponent<Cell>().ofType != "empty" && grid[x, y - 1].GetComponent<Cell>().ofType != "goop")
                            TransformTile(new Vector2(x, y), "goop");
                    //if(CheckIfCellExists(x+1,y+1))
                    //if(grid[x+1,y+1].GetComponent<Cell>().ofType != "empty" && grid[x+1,y+1].GetComponent<Cell>().ofType != "goop")
                    //TransformTile(new Vector2(x,y), "goop");
                    //	if(CheckIfCellExists(x+1,y-1))
                    //if(grid[x+1,y-1].GetComponent<Cell>().ofType != "empty" && grid[x-1,y-1].GetComponent<Cell>().ofType != "goop")
                    //	TransformTile(new Vector2(x,y), "goop");

                }

            }
        }
    }
    void TransformTile(Vector2 position, string newState)
    {
        if (CheckIfCellExists((int)position.x, (int)position.y))
        {
            //Check if the cell is valid
            Cell currentCell = grid[(int)position.x, (int)position.y].GetComponent<Cell>();
            currentCell.ofType = newState;
            currentCell.UpdateType();
        }
        else
        {
            print("Invalid cell.");
            //Cell is invalid, break and Restart.
        }
    }

    bool CheckIfCellExists(int x, int y)
    {
        if (x >= 0 && x < size.x && y >= 0 && y < size.y)
        {
            return (true);
        }
        else
            return (false);
    }
    void CheckNextBake()
    {
        nextBake = Mathf.Clamp(nextBake, 0, 3);
        if (nextBake > 0 && toBake < 4)
            nextBake -= Time.deltaTime;
        else
        {
            if (toBake > 0)
            {
                Rebake();
                toBake = 0;
                print("Rebaking.");
            }
            print("Already baked.");
        }
    }
    public void AddBakeTime(float add)
    {
        toBake++;
        if (nextBake <= 0)
            nextBake += add * 3;
        else
            nextBake += add;
    }

   

    public void GenerateGrid()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (y % 2 == 0)
                    grid[x, y] = Instantiate(sampleHex, transform.position + new Vector3(10 * x, 0, 7.5f * y), Quaternion.identity);
                else
                    grid[x, y] = Instantiate(sampleHex, transform.position + new Vector3(10 * x + 5f, 0, 7.5f * y), Quaternion.identity);

                grid[x, y].transform.SetParent(holderGrid.transform);
                grid[x, y].GetComponent<Cell>().ofType = "empty";
                grid[x, y].GetComponent<Cell>().UpdateType();
            }
        }
        Rebake();
    }

    public void Rebake()
    {
        navMesh.BuildNavMesh();
    }

    IEnumerator WinGame()
    {
        TurnOffGUI ();
        print("You win!");
        gameIsOn = false;
        loseGUI.SetActive(false);winGUI.SetActive(true);
        Vector3 objective = winGUI.transform.position - new Vector3(1000,0,0);
        while(winGUI.transform.position.x > objective.x) {
            winGUI.transform.position += new Vector3(-10,0,0);
            yield return null;
        }
        
        yield return null;
    }

    IEnumerator LoseGame()
    {
        TurnOffGUI ();
        print("You lost!");
        gameIsOn = false;

        loseGUI.SetActive(true);winGUI.SetActive(false);
        Vector3 objective = loseGUI.transform.position - new Vector3(1000,0,0);
        print("1");
        while(loseGUI.transform.position.x > objective.x) {
            loseGUI.transform.position += new Vector3(-10,0,0);
            print("2");
            yield return null;
        }
print("3");
        yield return null;
    }

    void TurnOffGUI () {
        GUI1.SetActive(false);
        GUI2.SetActive(false);
        GUI3.SetActive(false);
    }
}
