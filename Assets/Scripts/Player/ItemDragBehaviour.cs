using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragBehaviour : MonoBehaviour
{
    [SerializeField] public float dragForce = 10;
    [SerializeField] Camera playerCamera;

    private Rigidbody dragginigRigidbody;
    float distanceToRigidbody;
    Vector3 initialMousePosition;
    Vector3 initialRigidbodyPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCamera)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) //TODO: Change to Input System for event-based inputs. This works as a quick prototype
        {
            dragginigRigidbody = RaycastRigidbodyFromMousePointer();
        }
        if (Input.GetMouseButtonUp(0) && !(dragginigRigidbody is null))
        {
            dragginigRigidbody = null;
        }
    }
    void FixedUpdate()
    {
        if (dragginigRigidbody)
        {
            Vector3 mouseOffset = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToRigidbody)) - initialMousePosition;

            Vector3 force = (initialRigidbodyPosition + mouseOffset - dragginigRigidbody.transform.position) * dragForce;
            dragginigRigidbody.velocity = force * Time.fixedDeltaTime; //Direct access to velocity might be problematic. Adding force requires more time to combat overshooting
        }
    }

    Rigidbody RaycastRigidbodyFromMousePointer()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Items");
        bool isHit = Physics.Raycast(ray, out hit, 100000f, layerMask);
        if (isHit)
        {
            if (hit.collider.gameObject.tag == "Pickable")
            {
                distanceToRigidbody = Vector3.Distance(ray.origin, hit.point);
                initialMousePosition = playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToRigidbody));
                initialRigidbodyPosition = hit.collider.transform.position;
                return hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }
        return null;
    }
}
