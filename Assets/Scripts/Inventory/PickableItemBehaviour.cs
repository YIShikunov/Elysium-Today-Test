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

    private Collider itemCollider;

    void Start()
    {
        itemCollider = gameObject.GetComponent<Collider>();
        mesh = gameObject.GetComponent<MeshRenderer>();
        gameObject.tag = itemTag;
    }

    public void PickUp(InventoryBehaviour inventory)
    {
        //itemCollider.enabled = false;
        //mesh.enabled = false;
        Debug.Log("Caught inventory event");
        StartCoroutine(SendAction(inventory.info, ActionType.Add));
    }

    public void Drop(InventoryBehaviour inventory)
    {
        itemCollider.enabled = true;
        mesh.enabled = true;
        StartCoroutine(SendAction(inventory.info, ActionType.Remove));
    }

    IEnumerator SendAction(ItemInfo source, ActionType type)
    {
        string host = @"https://dev3r02.elysium.today/inventory/status";
        InventoryAction action = new InventoryAction() { action = type, source = source, target = info };
        string json = JsonUtility.ToJson(action);
        UnityWebRequest request = UnityWebRequest.Post(host, json);
        request.SetRequestHeader("auth", "\"BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6\"");
        Debug.Break();
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Break();
            Debug.Log(request.downloadHandler.text);
        }
    }

    void Update()
    {

    }
}
