using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode {
    idle,
    playing,
    levelEnd,
    gameOver
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    [Header("UI Elements")]
    public GameObject gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        S = this;

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
        gameOverUI.SetActive(false);
    }

    void StartLevel() {
        if(castle != null) {
            Destroy(castle);
        }
    

        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI() {
        uitLevel.text = "Level: "+(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet) {
            mode = GameMode.levelEnd;
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            Invoke("NextLevel", 2f);
        }

        if(mode == GameMode.gameOver){
            GameOverScreen();
        }
    }

    void NextLevel() {
        level++;
        if(level == levelMax) {
            mode = GameMode.gameOver;
            //level = 0;
            //shotsTaken = 0;
        }
        else {
            StartLevel();
        }
    }

    void GameOverScreen() {
        gameOverUI.SetActive(true);
    }

    static public void SHOT_FIRED() {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE() {
        return S.castle;
    }

    public void RestartGame() {
        gameOverUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
