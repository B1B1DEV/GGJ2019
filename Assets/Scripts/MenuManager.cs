using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioToolkit;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public Image overlay;
    public float blinkRate = 1.0f;

	// Use this for initialization
	void Start () {
        m_blackout = FindObjectOfType<Blackout>();
        m_blackout.FadeTo(Color.black, 0.0f);

        m_blackout.FadeTo(new Color(0.0f, 0.0f, 0.0f, 0.0f), 2.0f);

        AudioManager.Instance.Play("TechnoCrapMenu" , FindObjectOfType<Camera>().transform.position);
        AudioManager.Instance.Play("BrouhahaMenu", FindObjectOfType<Camera>().transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        if (Mathf.FloorToInt(Time.time / blinkRate) % 2 == 0)
        {
            overlay.enabled = true;
        }
        else
        {
            overlay.enabled = false;
        }

        if (Input.anyKeyDown && !m_oneTime)
        {
            m_oneTime = true;
            StartCoroutine(SwitchLevel());
        }
	}

    bool m_oneTime = false;
    Blackout m_blackout;

    IEnumerator SwitchLevel()
    {
        AudioManager.Instance.Stop("TechnoCrapMenu");
        AudioManager.Instance.Stop("BrouhahaMenu");
        m_blackout.FadeTo(Color.black, 2.0f);

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Level");
    }
}
