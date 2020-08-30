using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class PickableItemBehaviour : MonoBehaviour
{
    [SerializeField] private string itemTag = "Pickable";
    [SerializeField] private string itemName = "Item";
    [SerializeField] private Texture previewTexture;

    private MeshRenderer mesh;
    
    private Collider itemCollider;

    void Start()
    {
        itemCollider = gameObject.GetComponent<Collider>();
        mesh = gameObject.GetComponent<MeshRenderer>();
        gameObject.tag = itemTag;
    }

    public void PickUp()
    {
        itemCollider.enabled = false;
        mesh.enabled = false;
    }

    public void Drop()
    {
        itemCollider.enabled = true;
        mesh.enabled = true;
    }
    
    void Update()
    {
        
    }
}
