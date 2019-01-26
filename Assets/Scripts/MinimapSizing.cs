using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapSizing : MonoBehaviour {

    public Transform car; // car object
    public Transform house; // end zone
    public Transform levelPlane; // level plane to get the max size to display on minimap
    public float borderPercent = 20f; // percent of view to add around the 2 objects to obtain the final minimap square
    public float minCamSize = 50f; // minimum size of camera

    public RectTransform houseSprite;
    public RectTransform carSprite;

    Camera minimapCam;
    float maxSize;
    float housePosX;
    float housePosZ;

	// Use this for initialization
	void Start () {
        // Get the minimap Camera
        minimapCam = GetComponent<Camera>();
        if (minimapCam == null) Debug.Log("Cannot find minimap camera");

        // The max orthographicSize of the minimap Camera is the square of the minimum size (x or z) of the level base
        maxSize = Mathf.Pow(Mathf.Min(levelPlane.localScale.x, levelPlane.localScale.z), 2);

        // House position doesn't change during the game and are set at Start
        housePosX = house.position.x;
        housePosZ = house.position.z;

	}
	
	// Update is called once per frame
	void Update () {

        // orthographicSize is the half of the max distance on the right or forward axes + a safty border zone and limited to the max size of the level
        float minimapSize = Mathf.Min(Mathf.Max(Mathf.Abs(car.position.x - housePosX), Mathf.Abs(car.position.z - housePosZ)) * (1 + borderPercent / 100) / 2, maxSize);
        minimapCam.orthographicSize = Mathf.Max(minimapSize,minCamSize);

        // The new Camera position is in the middle of the car and the house (y position is kept) but clamped to never display the outside of the plane level
        float posX = Mathf.Clamp((car.position.x + housePosX) / 2, -maxSize + minimapSize, maxSize - minimapSize);
        float posZ = Mathf.Clamp((car.position.z + housePosZ) / 2, -maxSize + minimapSize, maxSize - minimapSize);
        transform.position = new Vector3(posX, transform.position.y, posZ);

        houseSprite.position = house.position;
        
        carSprite.position = car.position;
        carSprite.rotation = car.rotation;
        carSprite.Rotate(new Vector3(90, 0, 0));
    }
}
