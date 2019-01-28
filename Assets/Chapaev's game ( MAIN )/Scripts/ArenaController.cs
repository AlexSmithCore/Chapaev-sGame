using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{

    private GameController gc;

    void Start(){
        gc = GameController.instance;
    }

   private void OnTriggerExit(Collider other) {
        if(other.tag == "Chip"){
            other.gameObject.SetActive(false);
            ChipInfo ci = other.GetComponent<ChipInfo>();
            if(ci.chipTeam == gc.playerTeam){
                DeletePlayerChip(other.gameObject);
            } else {
                DeleteEnemyChip(other.gameObject);
            }
            gc.CheckForWin();
        }
    }

    private void DeletePlayerChip(GameObject chip){
        for(int i = 0; i < gc.playerChips.Count; i++){
            if(gc.playerChips[i] == chip){
                gc.playerChips.RemoveAt(i);
            }
        }
    }

    private void DeleteEnemyChip(GameObject chip){
        for(int i = 0; i < gc.enemyChips.Count; i++){
            if(gc.enemyChips[i] == chip){
                gc.enemyChips.RemoveAt(i);
            }
        }
    }
}
