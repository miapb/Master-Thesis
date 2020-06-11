using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMaterial : MonoBehaviour
{
    MeshRenderer meshRenderer;
    //public Texture albedoTexture;
    //public Texture defaultTexture;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void setSnow()
    {
        meshRenderer.material.color = Color.white;
    }
    
    public void setDefaultTerrain()
    {
        meshRenderer.material.color = new Color32(52, 89, 21, 255);
    }

}
