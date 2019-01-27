using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioToolkit;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.Play("TechnoCrapMenu");
        AudioManager.Instance.Play("BrouhahaMenu");
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown && !m_oneTime)
        {
            m_oneTime = true;
            StartCoroutine(SwitchLevel());
        }
	}

    bool m_oneTime = false;

    IEnumerator SwitchLevel()
    {
        AudioManager.Instance.Stop("TechnoCrapMenu");
        AudioManager.Instance.Stop("BrouhahaMenu");

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Level");
    }
}
