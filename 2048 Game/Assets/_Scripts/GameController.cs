using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static int ticker;

    [Header("-----Cells-----")]
    [SerializeField] GameObject fillPrefab;
    [SerializeField] Cell2048[] allCells;


    public static Action<string> slide;

    [Header("-----Display Score-----")]
    public int myScore;
    [SerializeField] Text scoreDisplay;

    [Header("-----Game Over-----")]
    [SerializeField] GameObject gameOverPanel;
    private int isGameOver;

    [Header("-----Colors-----")]
    public Color[] fillColors;

    [Header("-----Win-----")]
    [SerializeField] int winningValue;

    [SerializeField] GameObject winningPanel;

    [SerializeField] private bool hasWon;

    [SerializeField] private int hasWonCount;

    [Header("-----Restart-----")]
    [SerializeField] GameObject restartButton;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        /*else
        {
            DontDestroyOnLoad(gameObject);
        }*/
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartSpawnFill();
        StartSpawnFill();
    }

    // Update is called once per frame
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFill();
        }*/

        PressKey();
    }

    private void PressKey()
    {
        if (hasWon)
            return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetValue();
            slide("up");
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetValue();
            slide("left");
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetValue();
            slide("down");
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetValue();
            slide("right");
        }
    }

    private void SetValue()
    {
        ticker = 0;
        isGameOver = 0;
    }

    public void SpawnFill()
    {

        bool isFull = true;
        for(int i = 0; i < allCells.Length; i++)
        {
            if (allCells[i].fill == null)
            {
                isFull = false;
            }
        }

        if (isFull == true || hasWon)
        {
            return;
        }

        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);

        if (allCells[whichSpawn].transform.childCount != 0)
        {
            Debug.Log(allCells[whichSpawn] + " is already filled");
            SpawnFill();
            return;
        }

        float chance = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(chance);
        if(chance < 0.2f)
        {
            return;
        }
        else if (chance < 0.8f)
        {
            
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            //Debug.Log("SpawnFill: " + 2);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(2);
        }
        else
        {
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            //Debug.Log("SpawnFill: " + 4);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(4);
        }
    }

    public void StartSpawnFill()
    {

        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);

        /*if (allCells[whichSpawn].transform.childCount != 0)
        {
            Debug.Log(allCells[whichSpawn] + " is already filled");
            SpawnFill();
            return;
        }*/

        GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
        Debug.Log("StartSpawnFill: " + 2);
        Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
        allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
        tempFillComp.FillValueUpdate(2);
        
    }

    public void ScoreUpdate(int scoreIn)
    {
        myScore += scoreIn;
        scoreDisplay.text = myScore.ToString();
    }

    public void GameOverCheck()
    {
        isGameOver++;
        if(isGameOver >= 16)
        {
            restartButton.SetActive(false);

            gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void WinningCheck(int highestFill)
    {
        if (hasWonCount == 1)
            return;

        if (hasWon)
            return;

        if (highestFill == winningValue)
        {
            restartButton.SetActive(false);

            winningPanel.SetActive(true);
            hasWon = true;
            hasWonCount++;

            StartCoroutine(WinningDelay());
        }
    }

    IEnumerator WinningDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0f;
    }

    public void KeepPlaying()
    {
        hasWon = false;
        Time.timeScale = 1f;
        winningPanel.SetActive(false);
    }
}
