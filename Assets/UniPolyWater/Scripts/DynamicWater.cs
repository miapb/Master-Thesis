using UnityEngine;
using System.Collections;

public class DynamicWater : MonoBehaviour {

    Material waveMaterial;
    GlobalReferences globals;

	// Use this for initialization
	void Start ()
    {
        globals = GameObject.Find("Globals").GetComponent<GlobalReferences>();
        waveMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;
	}

	// Update is called once per frame
	void Update () {
        for(int i = 0; i < globals.wavesIsMoving.Length; i++)
        {
            if (globals.wavesIsMoving[i])
            {
                globals.collisionVectors[i].w = globals.waveTimers[i] * 0.4f;
                if (globals.collisionVectors[i].w > 1.0f)
                {
                    globals.wavesIsMoving[i] = false;
                    globals.collisionVectors[i].w = 0.0f;
                    globals.waveTimers[i] = 0.0f;
					return;
                }
                globals.waveTimers[i] += Time.deltaTime;
            }
        }
        waveMaterial.SetVectorArray("_CollisionVectors", globals.collisionVectors);
        waveMaterial.SetFloatArray("_CollisionWaveOffsets", globals.collisionWaveOffsets);
    }

    public void AddWave(Vector2 pos, float strenght, float offset)
    {
        globals.wavesIsMoving[globals.waveCounter] = true;
        globals.waveTimers[globals.waveCounter] = 0.0f;
        globals.collisionVectors[globals.waveCounter].x = pos.x;
        globals.collisionVectors[globals.waveCounter].y = pos.y;
        globals.collisionVectors[globals.waveCounter].z = strenght;
        globals.collisionVectors[globals.waveCounter].w = 0.0f;
        globals.collisionWaveOffsets[globals.waveCounter] = offset;
        globals.waveCounter++;
        if (globals.waveCounter >= 10)
        {
            globals.waveCounter = 0;
        }
    }
}
