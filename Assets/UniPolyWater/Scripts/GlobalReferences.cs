using UnityEngine;
using System.Collections;

public class GlobalReferences : MonoBehaviour
{

    public int waveCounter;
    public bool[] wavesIsMoving;
    public float[] waveTimers;
    public Vector4[] collisionVectors;
    public float[] collisionWaveOffsets;

    void Start()
    {
        waveCounter = 0;
        wavesIsMoving = new bool[10];
        waveTimers = new float[10];
        collisionVectors = new Vector4[10];
        collisionWaveOffsets = new float[10];

        for (int i = 0; i < collisionVectors.Length; i++)
        {
            collisionVectors[i] = new Vector4(0, 0, 0, 0);
        }
    }
}
