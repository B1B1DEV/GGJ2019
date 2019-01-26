using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
