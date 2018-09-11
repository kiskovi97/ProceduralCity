using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 distance = new Vector3(0, 10, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player!=null)
        {
            transform.position = player.position + distance;
            transform.rotation = Quaternion.LookRotation(player.position - transform.position);

            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, player.eulerAngles.z);
        }
	}
}
