using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceManager : MonoBehaviour
{
    public GameObject environment;
    public Text test;
    

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits= new List<ARRaycastHit>();
    private bool placed;
    private Button readyBtn;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager= FindObjectOfType<ARRaycastManager>();
        environment.SetActive(false);
        readyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
        readyBtn.interactable= false;
        placed = false;
    }

    // Update is called once per frame
    void Update()
    {
        var ray =  new Vector2(Screen.width / 2, Screen.height / 2);

        if(raycastManager.Raycast(ray, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            if (!placed)
            {
                transform.position = hitPose.position;
                transform.rotation = hitPose.rotation;


                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                    Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                    float preMagnitude = (touch1PrevPos - touch2PrevPos).magnitude;
                    float currentMagnitude = (touch1.position - touch2.position).magnitude;

                    float zoom = (currentMagnitude - preMagnitude) * 0.001f;
                    
                    zoom = Mathf.Clamp(zoom, 0.01f, 0.05f);
                    environment.transform.localScale = new Vector3(zoom, zoom, zoom);
                    test.text = zoom.ToString();

                }
            }

            if(!environment.activeInHierarchy)
            {
                environment.SetActive(true);
            }    
        } 
    }

    public void ClickToPlace()
    {
        if (environment.activeInHierarchy)
        {
            placed = true;
            readyBtn.interactable = true;
        }
        
    }

    public void ClickToReset()
    {
        placed = false;
        readyBtn.interactable = false;
    }
}
