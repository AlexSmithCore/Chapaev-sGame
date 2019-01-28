using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{

    [SerializeField]
    private bool dragged;

    private Camera cam;

    public LayerMask chipsMask;

    public GameObject selectedChip;

    [HideInInspector]
    public LineRenderer lr;

    private Vector3 mousePos;

    [SerializeField]
    private float distance;

    void Awake(){
        cam = FindObjectOfType<Camera>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    void Update(){
        if(GameController.instance.turn == GameController.instance.playerTeam){
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, chipsMask)){
                mousePos = hit.point;
                if(hit.collider.tag == "Chip"){
                    if(Input.GetMouseButton(0) && !dragged){
                        SelectChip(hit.collider.GetComponent<ChipInfo>());
                    }
                }
            }

            if(Input.GetMouseButtonUp(0)){
                if(distance > 3f){
                    ThrowChip(mousePos, selectedChip);
                } else {
                    DisableChip(selectedChip);
                }
            }

            if(dragged){
                lr.enabled = true;
                lr.SetPosition(0,selectedChip.transform.position);
                lr.SetPosition(1,mousePos);
                distance = (selectedChip.transform.position - mousePos).magnitude;
            }
        }
    }



    private void ThrowChip(Vector3 lastPos, GameObject chip){
        chip.transform.LookAt(lastPos);
        chip.GetComponent<Rigidbody>().AddForce(chip.transform.forward * (distance * 200f));
        DisableChip(chip);
        GameController.instance.Invoke("ChangeTurn", 2f);
    }

    private void SelectChip(ChipInfo chip){
        if(selectedChip != chip.gameObject && chip.chipTeam == GameController.instance.playerTeam){
            dragged = true;
            selectedChip = chip.gameObject;
            Renderer renderer = selectedChip.GetComponent<Renderer>();
            Material material = renderer.material;
            material.SetColor("_EmissiveColor", material.GetColor("_EmissiveColor") * 8f);
            lr.material.SetColor("_EmissiveColor", material.GetColor("_EmissiveColor") / 6);
        }
    }

    private void DisableAllChips(){
        Rigidbody[] chips = FindObjectsOfType<Rigidbody>();
        for(int i = 0; i < chips.Length; i++){
            DisableChip(chips[i].gameObject);
        }
    }

    private void DisableChip(GameObject chip){
        distance = 0;
        lr.enabled = false;
        if(chip != null){
            dragged = false;
            Renderer renderer = chip.GetComponent<Renderer>();
            Material material = renderer.material;
            material.SetColor("_EmissiveColor", chip.GetComponent<ChipInfo>().chipEmission);
            selectedChip = null;
        }
    }
}
