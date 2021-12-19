using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    MeshRenderer render;
    BoxCollider col;

    [SerializeField] Item item;

    public static event Action<Item> IsTakeItem;

    private void Start()
    {
        render = GetComponent<MeshRenderer>();
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsTakeItem?.Invoke(item);
            render.enabled = false;
            col.enabled = false;

            StartCoroutine(C_DelayToDestroy());
        }
    }

    IEnumerator C_DelayToDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}

[Serializable]
public enum ItemType
{
    RifleAmmo,
    HandgunAmmo,
    Health
}

[Serializable]
public class Item
{
    public ItemType itemType;
    public int amount;
}
