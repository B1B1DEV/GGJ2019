using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Transform levelBounds;

    [Header("VomitGauge")]
    public float hitEffectThreshold = 5.0f;
    public float vomitGaugeMax = 30.0f;

    [Header("Internal")]
    public MSVehicleControllerFree carPrefab;
    public MinimapSizing minimapPrefab;
    public RectTransform GUIPrefab;

    // Use this for initialization
    void Start ()
    {
        m_GUI = Instantiate<RectTransform>(GUIPrefab);

        m_blackout = FindObjectOfType<Blackout>();
        m_startPoints = FindObjectsOfType<StartPointController>();
        m_destinations = FindObjectsOfType<DestinationController>();

        if (m_startPoints.Length < 0)
        {
            Debug.LogError("No Start point !");
            return;
        }

        if (m_destinations.Length < 0)
        {
            Debug.LogError("No Destinations !");
            return;
        }

        m_currentStartPoint = m_startPoints[Random.Range(0, m_startPoints.Length)];
        m_currentDestination = m_destinations[Random.Range(0, m_destinations.Length)];

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(m_currentStartPoint.transform.position, Vector3.down, out hit, 100.0f, LayerMask.GetMask("Default", "Roads")))
        {
            m_car = Instantiate<MSVehicleControllerFree>(carPrefab, hit.point, m_currentStartPoint.transform.rotation);

            m_car.destinationReached += onCarReachedDestination;
            m_car.hitCollision += onCarHitCollision;

            m_minimap = Instantiate<MinimapSizing>(minimapPrefab);
            m_minimap.car = m_car.transform;
            m_minimap.house = m_currentDestination.transform;
            m_minimap.levelPlane = levelBounds;
        }

        if (m_car)
        {
            StartCoroutine(StartupSequence());
        }
    }

    IEnumerator StartupSequence()
    {
        m_blackout.FadeTo(Color.black, 0.0f);

        AudioManager.Instance.Play("TechnoCrap", m_car.transform.position + m_car.transform.forward * 20.0f);
        AudioManager.Instance.Play("Brouhaha", m_car.transform.position + m_car.transform.forward * 20.0f);
        yield return new WaitForSeconds(4.5f);
        AudioManager.Instance.Play("OpenDoor", m_car.transform.position);
        yield return new WaitForSeconds(0.4f);
        AudioManager.Instance.Play("SlamDoor", m_car.transform.position);

        AudioManager.Instance.Stop("Brouhaha");

        AudioManager.Instance.Stop("TechnoCrap"); // Fades

        for (int i = 0; i < 7; ++i)
        {
            AudioManager.Instance.Play("FootStep", m_car.transform.position - Vector3.up * 2.0f);
            yield return new WaitForSeconds(0.7f);
        }

        AudioManager.Instance.Stop("OpenDoor");
        AudioManager.Instance.Play("OpenDoor", m_car.transform.position);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.Play("CloseDoor", m_car.transform.position);
        AudioManager.Instance.GetSound("TechnoCrap").DestroyInstances();
        yield return new WaitForSeconds(1.5f);

        AudioManager.Instance.Play("Yawn", m_car.transform.position);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.Play("Hiccup", m_car.transform.position);
        yield return new WaitForSeconds(0.5f);

        AudioManager.Instance.Play("EngineStart", m_car.transform.position);
        yield return new WaitForSeconds(0.2f);
        m_blackout.FadeTo(new Color(0.0f, 0.0f, 0.0f, 0.0f), 2.0f);

        yield return new WaitForSeconds(1.3f);

        m_car.setThrustEnabled(true);

        //yield return new WaitUntil(() => m_avatar.GetState() != AvatarState.Start);
    }

    IEnumerator EndSequence()
    {
        m_car.setThrustEnabled(false);
        m_blackout.FadeTo(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator VomitSequence()
    {
        m_car.setThrustEnabled(false);
        m_blackout.FadeTo(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void onCarReachedDestination(MSVehicleControllerFree _car, DestinationController _destination)
    {
        if (m_gameOver)
            return;

        if (_destination == m_currentDestination)
        {
            m_gameOver = true;
            StartCoroutine(EndSequence());
        }
    }

    void onCarHitCollision(MSVehicleControllerFree _car, Collision _hit)
    {
        if (m_gameOver)
            return;

        Vector3 horizontalDirection = new Vector3(_hit.relativeVelocity.x, 0.0f, _hit.relativeVelocity.z);
        float hitStrength = Vector3.Dot(horizontalDirection.normalized, _hit.relativeVelocity);
        if (hitStrength > hitEffectThreshold)
        {
            m_vomitGauge += hitStrength;

            if (m_vomitGauge >= vomitGaugeMax)
            {
                m_gameOver = true;
                StartCoroutine(VomitSequence());
            }
        }
    }

    public float GetVomitGauge()
    {
        return m_vomitGauge;
    }

    private bool m_gameOver = false;
    private float m_vomitGauge = 0.0f;

    private StartPointController[] m_startPoints;
    private DestinationController[] m_destinations;

    private StartPointController m_currentStartPoint;
    private DestinationController m_currentDestination;

    private MSVehicleControllerFree m_car;
    private Blackout m_blackout;
    private MinimapSizing m_minimap;
    private RectTransform m_GUI;
}
