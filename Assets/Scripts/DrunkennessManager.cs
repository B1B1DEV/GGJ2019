using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkennessManager : MonoBehaviour
{
    public float timeToTotalDrunkenness = 60.0f;

    [Range(0f, 1f)]
    [SerializeField]
    private float drunkenness = 0.0f;

    public static float GetDrunkenness()
    {
        return Instance().drunkenness;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!FindObjectOfType<GameManager>().isGameOver())
        {
            drunkenness = Mathf.Clamp01(drunkenness + Time.deltaTime / timeToTotalDrunkenness);
        }
    }

    public static DrunkennessManager Instance()
    {
        if (!m_instance)
        {
            m_instance = FindObjectOfType<DrunkennessManager>();
        }
        return m_instance;
    }
    private static DrunkennessManager m_instance;
}
