using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class move_homyaka : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ARRaycastManager _raycastManager;

    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();

    void Start()
    {
        GameObject arso = GameObject.Find("AR Session Origin");
        Debug.Log(arso);
        ARRaycastManager arrm = arso.GetComponent(typeof(ARRaycastManager)) as ARRaycastManager;
        Debug.Log(arrm);
        _raycastManager = arrm;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (_raycastManager.Raycast(touch.position, _raycastHits))
        {
            // Beginning of the touch, this triggers when the finger first touches the screen
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                // _raycastHits[0].pose.position
                float   speed = 2;
                Vector3 modified_pos = new Vector3(
                    _raycastHits[0].pose.position.x,
                    transform.position.y,
                    _raycastHits[0].pose.position.z
                );
                Vector3 diff      = modified_pos - transform.position;
                Vector3 direction = diff;
                Vector3 velocity  = speed * direction;

                GetComponent<Rigidbody>().velocity = velocity;
            }

        }
    }
}
