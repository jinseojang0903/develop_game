using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotsParent; //Slot들이 담겨있는 부모 오브젝트
    private Inventory inventory;
    private InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = slotsParent.GetComponentsInChildren<InventorySlot>();
    }

    //ui 업데이트 박병주 10련아
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}