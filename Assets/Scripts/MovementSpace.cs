using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpace : MonoBehaviour
{
    public Material[] mat = new Material[2];
    Renderer rend;

    public void SetMaterial(string key)
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = true;
        if (key.Contains("Attack") || key.Contains("EnPassant")){
            rend.material = mat[0];
        }
        else
        {
            rend.material = mat[1];
        }
    }
}
