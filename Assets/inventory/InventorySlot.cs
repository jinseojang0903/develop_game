using UnityEngine;
using UnityEngine.UI; // Image, Button을 사용하기 위해 필요

// 이 스크립트는 Slot_0, Slot_1, ... 각각의 오브젝트에 붙입니다.
public class InventorySlot : MonoBehaviour
{
    public Image icon; // 슬롯의 아이콘 이미지 (인스펙터에서 연결)
    public Button removeButton; // 아이템 제거 버튼 (선택 사항)

    private ItemData item; // 현재 슬롯에 할당된 아이템

    void Awake()
    {
        // icon이 할당되지 않았다면 자식에서 찾아서 자동 할당
        // Slot 오브젝트의 자식으로 아이콘을 표시할 Image 오브젝트를 두는 구조를 권장
        if (icon == null)
        {
            // 자식 중에 Image 컴포넌트를 찾지만, 자기 자신은 제외
            Image foundIcon = GetComponentInChildren<Image>(true);
            if (foundIcon != null && foundIcon.gameObject != this.gameObject)
            {
                icon = foundIcon;
            }
        }
    }

    // 슬롯에 아이템 정보를 채우는 함수
    public void AddItem(ItemData newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true; // 아이콘 활성화
        if(removeButton != null) removeButton.interactable = true;
    }

    // 슬롯을 비우는 함수
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false; // 아이콘 비활성화
        if(removeButton != null) removeButton.interactable = false;
    }

    // 아이템 제거 버튼을 눌렀을 때 호출될 함수
    public void OnRemoveButton()
    {
        if (item != null)
        {
            Inventory.instance.Remove(item);
        }
    }
}
