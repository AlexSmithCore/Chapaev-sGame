using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipInfo : MonoBehaviour
{
    public int chipTeam = 0;

    public Color chipEmission;

    void Start(){
        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;
        chipEmission = material.GetColor("_EmissiveColor");
    }
}
