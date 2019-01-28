using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimer
{
    public float sec;
    public int min;
    public GameTimer(float newSec, int newMin){
        sec=newSec;
        min=newMin;
    }
}

public class GameController : MonoBehaviour
{
    #region  Singleton
    public static GameController instance;

    void Awake(){
        instance = this;
    }

    #endregion

    public bool gamePause;

    public GameTimer gameTimer;

    public List<GameObject> playerChips = new List<GameObject>();
    public List<GameObject> enemyChips = new List<GameObject>();

    public int playerTeam = -1;
    public int turn = -1;

    private int victoryTeam = -1;
    private UIController uc;

    void Start(){
        uc = GetComponent<UIController>();
        gamePause = true;
    }

    void Update(){
        if(!gamePause){
            gameTimer.sec += uc.ChangeTime();
            if(gameTimer.sec >= 60){
                gameTimer.sec = 0;
                gameTimer.min++;
            }
        }
    }

    public void RedTeam(){
        playerTeam = 1;
        uc.CloseChooseWindow();
        
        ChangeChips();
        ChangeTurn();
    }

    public void BlueTeam(){
        playerTeam = 0;
        uc.CloseChooseWindow();

        ChangeChips();
        ChangeTurn();
    }

    private void ChangeChips(){
        ChipInfo[] c = FindObjectsOfType<ChipInfo>();

        for(int i = 0; i < c.Length; i++){
            if(c[i].chipTeam == playerTeam){
                playerChips.Add(c[i].gameObject);
            } else {
                enemyChips.Add(c[i].gameObject);
            }
        }
    }

    public void CheckForWin(){
        uc.ChangeCheekersCount();
        if(enemyChips.Count <= 0 && playerChips.Count <= 0){
            victoryTeam = -1;
            gamePause = true;
            uc.ShowWinnerPanel(victoryTeam);
            return;
        }

        if(enemyChips.Count <= 0){
            victoryTeam = playerTeam;
            gamePause = true;
            uc.ShowWinnerPanel(victoryTeam);
            return;
        }

        if(playerChips.Count <= 0){
            victoryTeam--;
            if(victoryTeam < 0){
                victoryTeam = 1;
            }
            gamePause = true;
            uc.ShowWinnerPanel(victoryTeam);
            return;
        }
    }


    public void ChangeTurn(){
        if(!gamePause){
            turn++;
            if(turn >= 2){
                turn = 0;
            }

            uc.ChangeTurn();

            if(turn != playerTeam){
                GetComponent<ArtificialIntelligence>().StartMove();
            }
        }
    }

    public void Restart(){
        Application.LoadLevel(0);
    }

    public void Exit(){
        Application.Quit();
    }
}
