using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<MeshRenderer>().enabled = false;
	}
	

}
