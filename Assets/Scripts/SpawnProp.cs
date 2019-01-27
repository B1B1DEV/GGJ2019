using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProp : MonoBehaviour {

    [SerializeField]
    private GameObject propsPool;

    private List<GameObject> propsList;
    private List<Transform> zoneList;

    // Use this for initialization
    void Start ()
    {
        BuildPropsList();
        BuildZoneList();

        // Affectation
        Affect();
    }
	

    // Build list
    void BuildPropsList()
    {
        propsList = new List<GameObject>(); 

        for (int i = 0; i < propsPool.transform.childCount; i++)
        {
            propsList.Add(propsPool.transform.GetChild(i).gameObject);
        }
    }

    void BuildZoneList()
    {
        zoneList = new List<Transform>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            zoneList.Add(this.transform.GetChild(i));
        }
    }

    // randomly picks an object from the pool
    void Affect()
    {
        foreach (Transform zone in zoneList)
        {
            // Random pick
            int rn = Random.Range(0, propsList.Count-1);
            //propsList[rn].transform.SetParent(this.transform);
            propsList[rn].transform.SetPositionAndRotation(zone.transform.position, propsList[rn].transform.rotation);

            if (propsList.Count>1)
                propsList.RemoveAt(rn);
        }
    }
}
