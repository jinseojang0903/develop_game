using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<ItemData> items = new List<ItemData>();
    
    public int space = 9;

    /// <param name="item">추가할 아이템 데이터</param>
    /// <returns>추가 성공 여부</returns>
    public bool Add(ItemData item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room in inventory.");
            return false;
        }

        items.Add(item);

        onItemChangedCallback?.Invoke();

        return true;
    }

    /// <param name="item">제거할 아이템 데이터</param>
    public void Remove(ItemData item)
    {
        items.Remove(item);

        //아이템이 변경되었음을 알림
        onItemChangedCallback?.Invoke();
    }
}
