using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDrop : MonoBehaviour
{
	public DynamicWater dynamicWater;
	public GameObject particlePF;

    // Update is called once per frame
    void Update()
    {
		if(transform.position.y <= 0)
		{
			dynamicWater.AddWave(new Vector2(transform.position.x, transform.position.z), 0.2f, 0.4f);
			GameObject gO = GameObject.Instantiate(particlePF, transform.position, Quaternion.identity);
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.velocity = Vector3.zero;
			rb.MovePosition(new Vector3(Random.Range(0, 40), Random.Range(45, 60), Random.Range(-30, 50)));
			rb.MoveRotation(Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
		}
    }
}
