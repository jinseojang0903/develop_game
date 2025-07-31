using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    // 인스펙터 창에서 테스트할 아이템을 연결해주세요.
    public ItemData testItem;

    void Update()
    {
        // 키보드의 '1' 키를 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (testItem != null)
            {
                // 인벤토리에 테스트 아이템을 추가합니다.
                bool success = Inventory.instance.Add(testItem);

                if (success)
                {
                    Debug.Log(testItem.itemName + " 아이템을 추가했습니다.");
                }
                else
                {
                    Debug.Log("인벤토리가 가득 찼습니다.");
                }
            }
            else
            {
                Debug.LogWarning("테스트할 아이템이 지정되지 않았습니다!");
            }
        }
    }
}
