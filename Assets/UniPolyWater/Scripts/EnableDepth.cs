using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDepth : MonoBehaviour
{
   
	private void Start()
	{
		gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}
}
