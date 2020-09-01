using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddInventoryEvent : UnityEvent<InventoryBehaviour>
{
    public new void AddListener(UnityAction<InventoryBehaviour> call)
    {
        Debug.Log("Subscribed to Inventory");
        base.AddListener(call);
    }
    public new void RemoveListener(UnityAction<InventoryBehaviour> call)
    {
        Debug.Log("Unsubscribed from Inventory");
        base.RemoveListener(call);
    }
}

public class InventoryBehaviour : MonoBehaviour
{

    public AddInventoryEvent AddToInventory;
    public ItemInfo info = new ItemInfo { id = "1234123", itemName = "Cylinder", type = "backpack" };
    [HideInInspector] public bool drawGUI;
    private string guiName;

    public List<PickableItemBehaviour> inventory;

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

    private void OnMouseDown()
    {
        if (inventory.Count > 0)
        {
            inventory[inventory.Count - 1].Drop(this);
        }
    }
}
