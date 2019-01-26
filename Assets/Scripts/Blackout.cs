using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    public Image image;

    public void FadeTo(Color _color, float _time)
    {
        if (_time == 0.0f)
        {
            image.color = _color;
        }

        m_startColor = image.color;
        m_target = _color;
        m_timer = 0.0f;
        m_transitionTime = _time;
    }

	// Use this for initialization
	void Start () {
        m_target = image.color;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_transitionTime == 0.0f)
        {
            image.color = m_target;
        }
        else
        {
            m_timer += Time.deltaTime;
            m_timer = Mathf.Clamp(m_timer, 0.0f, m_transitionTime);

            float t = m_timer / m_transitionTime;
            image.color = Color.Lerp(m_startColor, m_target, Ease.QuadInOut(t));
        }
	}

    Color m_target;
    Color m_startColor;
    float m_timer = 0.0f;
    float m_transitionTime = 0.0f;
}
