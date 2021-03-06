﻿using System.Collections;
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

    private PickableItemBehaviour pickable;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (!playerCamera)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) //TODO: Change to Input System for event-based inputs. This works as a quick prototype
        {
            dragginigRigidbody = RaycastRigidbodyFromMousePointer();
            if (dragginigRigidbody)
            {
                pickable = dragginigRigidbody.gameObject.GetComponent<PickableItemBehaviour>();
                if (pickable)
                {
                    pickable.dragging = true;
                }
            }

        }
        if (Input.GetMouseButtonUp(0) && !(dragginigRigidbody is null))
        {
            if (pickable)
            {
                pickable.dragging = false;
                pickable = null;
            }
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
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Items");
        bool isHit = Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask);
        if (isHit)
        {
            if (hit.collider.gameObject.CompareTag("Pickable"))
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
