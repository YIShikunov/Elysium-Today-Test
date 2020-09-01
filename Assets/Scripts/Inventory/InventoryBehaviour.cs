using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddInventoryEvent : UnityEvent<InventoryBehaviour>
{
}

public class InventoryBehaviour : MonoBehaviour
{

    public AddInventoryEvent AddToInventory;
    public ItemInfo info = new ItemInfo { id = "1234123", itemName = "Cylinder", type = "backpack" };
    private bool drawGUI;
    private string guiName;

    private List<PickableItemBehaviour> inventory;

    void Start()
    {
        AddToInventory = new AddInventoryEvent();
        inventory = new List<PickableItemBehaviour>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AddToInventory.Invoke(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var pickable = other.gameObject.GetComponent<PickableItemBehaviour>();
        if (!(pickable is null))
        {
            Debug.Log("Subscribed to Inventory");
            AddToInventory.AddListener(pickable.PickUp);
            drawGUI = true;
            guiName = pickable.info.itemName;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var pickable = other.gameObject.GetComponent<PickableItemBehaviour>();
        if (!(pickable is null))
        {
            Debug.Log("Unsubscribed to Inventory");
            AddToInventory.RemoveListener(pickable.PickUp);
            drawGUI = false;
        }
    }

    private void OnGUI()
    {
        if (drawGUI)
        {
            string text = $"Release mouse to add {guiName} to container";
            GUI.color = Color.black;
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.width / 2 - 25, 150, 50), text);
        }
    }

}
