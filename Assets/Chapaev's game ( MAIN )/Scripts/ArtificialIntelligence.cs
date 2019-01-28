using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour
{

    private GameController gc;
    private RayCaster rc;
    private float betterDistance;
    private Transform target, betterChip;
    public Transform bestThrow;

    private float lastDistance;
    private Vector3 lastDir;

    void Start(){
        gc = GetComponent<GameController>();
        rc = GetComponent<RayCaster>();
    }

    public void StartMove(){
        if(!ChooseRandom()){
            Invoke("FindNearestChip", Random.Range(.5f,1.5f));
        } else {
            Invoke("SelectRandom", Random.Range(.5f,1.5f));
        }
    }

    private bool ChooseRandom(){
        float rand = Random.Range(0f,100f);
        if(rand >= 35f){
            return true;
        }
        return false;
    }

    private void SelectRandom(){
        target = gc.playerChips[Random.Range(0,gc.playerChips.Count)].transform;
        betterChip = gc.enemyChips[Random.Range(0,gc.enemyChips.Count)].transform;
        ChooseBetterChip();
    }

    private void FindNearestChip(){
        betterDistance = Mathf.Infinity;
        float distance = 0;
        for(int e = 0; e < gc.enemyChips.Count; e++){
            for(int p = 0; p < gc.playerChips.Count; p++){
                distance = (gc.enemyChips[e].transform.position - gc.playerChips[p].transform.position).magnitude;
                if(distance < betterDistance){
                    betterDistance = distance;
                    target = gc.playerChips[p].transform;
                    betterChip = gc.enemyChips[e].transform;
                }
            }
        }
        ChooseBetterChip();
    }

    private void ChooseBetterChip(){
        Renderer renderer = betterChip.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetColor("_EmissiveColor", material.GetColor("_EmissiveColor") * 8f);
        rc.lr.material.SetColor("_EmissiveColor", material.GetColor("_EmissiveColor") / 6);
        Invoke("DrawLine", Random.Range(1f,2f));
    }

    private void DrawLine(){
        DisableBetterChip();
        rc.lr.SetPosition(0,betterChip.position);
        rc.lr.SetPosition(1,CalculateBestThrow().position);
        Invoke("ThrowChip", Random.Range(.5f,2f));
    }

    private Transform CalculateBestThrow(){
        lastDir = target.position - betterChip.position;
        lastDistance = lastDir.magnitude;
        Vector3 randPos = new Vector3(Random.Range(0.1f,1.25f),0,Random.Range(0.1f,1.25f));
        /*if(lastDistance >= 20f){
            float rand = Random.Range(1f, 2f);
            bestThrow.position = (target.position / rand) + randPos;
            return bestThrow;
        }*/
        if(lastDistance <= 6f){
            float rand = Random.Range(1.5f, 1.75f);
            bestThrow.position = (target.position + (lastDir * rand)) + randPos;
            return bestThrow;
        }
        bestThrow.position = target.position + randPos;
        return bestThrow;
    }
    
    private void DisableBetterChip(){
        rc.lr.enabled = true;
        Renderer renderer =betterChip.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetColor("_EmissiveColor", betterChip.GetComponent<ChipInfo>().chipEmission);
    }

    private void ThrowChip(){
        rc.lr.enabled = false;
        betterChip.LookAt(bestThrow.position);
        float distance = (betterChip.position - bestThrow.position).magnitude;
        betterChip.GetComponent<Rigidbody>().AddForce(betterChip.forward * (distance * 200f));
        gc.Invoke("ChangeTurn", 2f);
    }
}
