using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraRotation : MonoBehaviour
{

    private float rotRange = 0f;
    private float camRotation = 0f;

    void Update(){
        if(GameController.instance.turn == 0){
            rotRange = 0;
        } else {
            rotRange = 180f;
        }

        camRotation = Mathf.Lerp(camRotation, rotRange, .375f);
    }

    void LateUpdate(){
        transform.eulerAngles = Vector3.up * camRotation;
    }
}
