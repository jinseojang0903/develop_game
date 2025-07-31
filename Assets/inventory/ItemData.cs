using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    //아이템의 고유 ID
    public int itemID;
    
    //아이템 이름
    public string itemName;

    //아이템 설명
    [TextArea]
    public string description;

    //인벤토리 슬롯에 표시될 아이콘
    public Sprite itemIcon;

    //아이템 타입
    public enum ItemType { Equipment, Consumable, Material }
    public ItemType itemType;

    //아이템이 겹쳐지는 최대 개수
    public int maxStack = 1;
}