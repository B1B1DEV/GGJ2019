using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DrunkenPP : MonoBehaviour {

    float d;
    public PostProcessVolume pp_notdrunk;
    public PostProcessVolume pp_drunk;

	// Use this for initialization
	void Start () {
        pp_notdrunk.weight = 1;
        pp_drunk.weight = 0;
	}
	
	// Update is called once per frame
	void Update () {

        d = DrunkennessManager.GetDrunkenness();

        pp_drunk.weight = Mathf.Min(1f, d);
		
	}
}
