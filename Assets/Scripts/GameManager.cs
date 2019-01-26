using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform levelBounds;

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
        if (Physics.Raycast(m_currentStartPoint.transform.position, Vector3.down, out hit, 100.0f, LayerMask.GetMask("Default")))
        {
            m_car = Instantiate<MSVehicleControllerFree>(carPrefab, hit.point, m_currentStartPoint.transform.rotation);

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

        yield return new WaitForSeconds(1.0f);

        m_blackout.FadeTo(new Color(0.0f, 0.0f, 0.0f, 0.0f), 2.0f);

        yield return new WaitForSeconds(1.5f);

        m_car.setThrustEnabled(true);

        //yield return new WaitUntil(() => m_avatar.GetState() != AvatarState.Start);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private StartPointController[] m_startPoints;
    private DestinationController[] m_destinations;

    private StartPointController m_currentStartPoint;
    private DestinationController m_currentDestination;

    private MSVehicleControllerFree m_car;
    private Blackout m_blackout;
    private MinimapSizing m_minimap;
    private RectTransform m_GUI;
}
