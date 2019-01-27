using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitGaugeController : MonoBehaviour
{
    public Transform liquid;

	// Use this for initialization
	void Start () {
        Vector3 scale = liquid.localScale;
        scale.y = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        Vector3 scale = liquid.localScale;
        scale.y = gameManager.getVomitGauge() / gameManager.vomitGaugeMax;
        liquid.localScale = scale;
    }
}
