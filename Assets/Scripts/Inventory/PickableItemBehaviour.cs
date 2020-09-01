using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class PickableItemBehaviour : MonoBehaviour
{
    private struct InventoryAction
    {
        public ItemInfo target;
        public ItemInfo source;
        public ActionType action;
    }

    public enum ActionType
    {
        Add,
        Remove
    }

    [SerializeField] private string itemTag = "Pickable";
    [SerializeField] public ItemInfo info = new ItemInfo 
    { 
        id = "123", 
        itemName = "Cube", 
        type = "cube" 
    };
    [SerializeField] public Texture previewTexture;

    private MeshRenderer mesh;
    private Rigidbody rb;
    private Collider itemCollider;

    public bool dragging = false;

    void Start()
    {
        itemCollider = gameObject.GetComponent<Collider>();
        mesh = gameObject.GetComponent<MeshRenderer>();
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.tag = itemTag;
    }

    public void PickUp(InventoryBehaviour inventory)
    {
        if (!dragging)
        {
            return;
        }
        StartCoroutine(SendAction(inventory.info, ActionType.Add));
        itemCollider.enabled = false;
        rb.isKinematic = true;
        mesh.enabled = false;
        gameObject.transform.SetParent(inventory.transform.parent.transform);
        inventory.AddToInventory.RemoveListener(PickUp);
        inventory.inventory.Add(this);
        inventory.drawGUI = false;
    }

    public void Drop(InventoryBehaviour inventory)
    {
        StartCoroutine(SendAction(inventory.info, ActionType.Remove));
        itemCollider.enabled = true;
        mesh.enabled = true;
        rb.isKinematic = false;
        gameObject.transform.SetParent(null);
        inventory.inventory.Remove(this);
    }

    IEnumerator SendAction(ItemInfo source, ActionType type)
    {
        string host = @"https://dev3r02.elysium.today/inventory/status";
        InventoryAction action = new InventoryAction() { action = type, source = source, target = info };
        string json = JsonUtility.ToJson(action);
        UnityWebRequest request = UnityWebRequest.Post(host, json);
        request.SetRequestHeader("auth", "\"BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6\"");
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    void Update()
    {

    }
}
