using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryBehaviour : MonoBehaviour
{

    public static UnityEvent AddToInventory;

    void Start()
    {
        AddToInventory = new UnityEvent();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AddToInventory.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var pickable = other.gameObject.GetComponent<PickableItemBehaviour>();
        if (!(pickable is null))
        {
            Debug.Log("Subscribed to Inventory");
            AddToInventory.AddListener(pickable.PickUp);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var pickable = other.gameObject.GetComponent<PickableItemBehaviour>();
        if (!(pickable is null))
        {
            Debug.Log("Unsubscribed to Inventory");
            AddToInventory.RemoveListener(pickable.PickUp);
        }
    }

    void OnTriggerStay(Collider other)
    {

    }
}
