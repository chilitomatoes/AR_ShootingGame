using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARRaycastPlace : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject objectToPlace;

    public Camera arCam;

    private List<ARRaycastHit> m_hits = new List<ARRaycastHit>();
    private GameObject placedObj;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if(raycastManager.Raycast(Input.GetTouch(0).position, m_hits))
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && placedObj == null)
            {
                placedObj = Instantiate(objectToPlace, m_hits[0].pose.position, Quaternion.identity);
                       
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && placedObj != null)
            {

                placedObj.transform.position = m_hits[0].pose.position;
            }
            //else if (Input.touchCount == 2)
            //{
            //    Touch touch1 = Input.GetTouch(0);
            //    Touch touch2 = Input.GetTouch(1);

            //    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            //    Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            //    float preMagnitude = (touch1PrevPos - touch2PrevPos).magnitude;
            //    float currentMagnitude = (touch1.position - touch2.position).magnitude;

            //    float zoom = (currentMagnitude - preMagnitude) * 0.01f;
            //    zoom = Mathf.Clamp(zoom, 0.01f, 0.05f);
            //    placedObj.transform.localScale = new Vector3(zoom, zoom, zoom);
            //}
        }
    }
}
