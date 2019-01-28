using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    GameController gc;

    public Transform cheekersPanel,winnerPanel,turnPanel,timerPanel, welcomePanel, choosePanel;

    private int blueCount;
    private int redCount;

    private int playerCount;

    private string evaluation;

    public Color blueColor, blueShadow, redColor, redShadow;

    void Start(){
        gc = GetComponent<GameController>();
    }

    public void CloseChooseWindow(){
        gc.gamePause = false;
        choosePanel.gameObject.SetActive(false);
    }

    public void NextPage(){
        welcomePanel.gameObject.SetActive(false);
        choosePanel.gameObject.SetActive(true);
    }

    public float ChangeTime(){
        timerPanel.gameObject.SetActive(!gc.gamePause);
        timerPanel.GetChild(3).GetComponent<Text>().text = (int)gc.gameTimer.sec + " sec";
        timerPanel.GetChild(2).GetComponent<Text>().text = gc.gameTimer.min + "";
        return Time.deltaTime;
    }

    public void ChangeTurn(){
        if(gc.turn == 0){
            turnPanel.GetChild(0).GetComponent<Image>().color = blueColor;
            turnPanel.GetChild(0).GetComponent<Shadow>().effectColor = blueShadow;
            turnPanel.GetChild(1).GetComponent<Text>().text = "BLUE TURN";
        } else {
            turnPanel.GetChild(0).GetComponent<Image>().color = redColor;
            turnPanel.GetChild(0).GetComponent<Shadow>().effectColor = redShadow;
            turnPanel.GetChild(1).GetComponent<Text>().text = "RED TURN";
        }
    }

    public void ChangeCheekersCount(){
        if(gc.playerTeam == 0){
            blueCount = gc.playerChips.Count;
            redCount = gc.enemyChips.Count;
            playerCount = blueCount;
        } else {
            blueCount = gc.enemyChips.Count;
            redCount = gc.playerChips.Count;
            playerCount = redCount;
        }

        cheekersPanel.GetChild(2).GetComponent<Text>().text = blueCount + "";
        cheekersPanel.GetChild(3).GetComponent<Text>().text = redCount + "";

        UpdateCheekers();
    }

    private void UpdateCheekers(){
        for(int i = 0; i < cheekersPanel.GetChild(0).childCount; i++){
            if(blueCount <= i){
                cheekersPanel.GetChild(0).GetChild(i).gameObject.SetActive(false);
            } else {
                cheekersPanel.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }

        for(int i = 0; i < cheekersPanel.GetChild(1).childCount; i++){
            if(redCount <= i){
                cheekersPanel.GetChild(1).GetChild(i).gameObject.SetActive(false);
            } else {
                cheekersPanel.GetChild(1).GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void ShowWinnerPanel(int winner){
        winnerPanel.gameObject.SetActive(true);
        winnerPanel.GetChild(1).GetChild(0).GetComponent<Text>().text = "Time passed: " + gc.gameTimer.min + " min " + (int)gc.gameTimer.sec + " sec";
        if(winner == -1){
            winnerPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "Draw!";
            winnerPanel.GetChild(1).GetChild(1).gameObject.SetActive(false);
            winnerPanel.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "D+";
            return;
        }

        if(winner == 0){
            winnerPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "Blue win!";
            winnerPanel.GetChild(1).GetChild(1).GetComponent<Text>().text = "Remaining chips: " + playerCount + "x";
            winnerPanel.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = CalculateEvaluation();
        } else {
            winnerPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "Red win!";
            winnerPanel.GetChild(1).GetChild(1).GetComponent<Text>().text = "Remaining chips: " + playerCount + "x";
            winnerPanel.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = CalculateEvaluation();
        }
    }

    private string CalculateEvaluation(){
        switch(playerCount){
            case 8:
              return "A";
            case 7:
              return "A-";
            case 6:
              return "B+";
            case 5:
              return "B";
            case 4:
              return "C+";
            case 3:
              return "C";
            case 2:
              return "D+";
            case 1:
              return "D";
            case 0:
              return "F";
        }
        return null;
    }

}
