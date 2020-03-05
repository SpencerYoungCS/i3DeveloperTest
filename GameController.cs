using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject currentCar;
    public GameObject partsList;
    public GameObject buttonPrefab;
    public GameObject carLabel;
    public GameObject partsListGUI;
    private Vector3 defaultCameraPosition;
    private Quaternion defaultCameraRotation;
    public bool lookingAtPart = false;

    // This is used for Rotate Script in conjunction with Mouseover (to disable emissions)
    public bool isRotating;

    void Start()
    {
        // Set gameobject to the prefab of the current car in the scene
        currentCar = GameObject.FindGameObjectWithTag("Car");

        // Save Current Camera Position for Reset Function
        defaultCameraPosition = Camera.main.transform.position;
        defaultCameraRotation = Camera.main.transform.rotation;

        // Set Top Label
        carLabel.GetComponent<Text>().text = currentCar.name + " Overview";
        AddModifiers(currentCar);


    }

    // This will add colliders on all the children and apply my appropriate scripts
    // That way, the developer won't have to add these components on every car in the future
    void AddModifiers(GameObject carToModify)
    {
        // Adding rotate script to the body of the car. can be used with any as long
        // as naming convention is consistent with "car name +Body"
        GameObject carBody = GameObject.Find(currentCar.name + "Body");
        carBody.AddComponent<RotateCarScript>();

        for (int i = 0; i < carToModify.transform.childCount; i++)
        {
            GameObject child = carToModify.transform.GetChild(i).gameObject;

            // Exclude OnPartClick on Body since our RotateScript will take care of it
            if (child != carBody)
                child.AddComponent<OnPartClick>();

            // Void the glow Components (Glow Effects aren't parts)
            if (!child.name.Contains("Glow"))
            {
                child.AddComponent<MeshCollider>();
                child.AddComponent<MouseOverScript>();

                // Add it to the list
                AddToList(child.name, child);
            }
        }
    }

    // Function that adds a button (prefab) to my button list
    void AddToList(string partName, GameObject objRef)
    {
        GameObject btnObj = Instantiate(buttonPrefab);
        btnObj.transform.SetParent(partsList.transform, false);
        Button btn = btnObj.GetComponent<Button>();
        // Adjusting the name of the part by removing the car's name from it
        btn.GetComponentInChildren<Text>().text = partName.Remove(0, currentCar.name.Length);
        btn.onClick.AddListener(objRef.GetComponent<ILookAtPart>().LookAtPart); 
    }

    public void resetCameraPosition()
    {
        Camera.main.transform.position = defaultCameraPosition;
        Camera.main.transform.rotation = defaultCameraRotation;
    }

}
