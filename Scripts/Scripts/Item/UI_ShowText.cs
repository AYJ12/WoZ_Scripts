using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShowText : MonoBehaviour
{
    private TextMeshProUGUI _infoActionText;
    private TextMeshProUGUI _pickUpActionText;
    private TextMeshProUGUI _useActionText_1;
    private TextMeshProUGUI _useActionText_2_1;
    private TextMeshProUGUI _useActionText_2_2;
    private TextMeshProUGUI _unablePickUpActionText;
    private Image _infoActionBg;
    private Image _infoActionE_Bg;
    private Image _infoActionE;
    private Image _pickUpActionBg;
    private Image _useActionBg_1;
    private Image _useActionBg_2_1;
    private Image _useActionBg_2_2;
    private Image _unablePickUpActionBg;

    private void Start()
    {
        TextMeshProUGUI[] text = GetComponentsInChildren<TextMeshProUGUI>(true);
        _infoActionText = text[0];
        _pickUpActionText = text[1];
        _unablePickUpActionText = text[2];
        _useActionText_1 = text[3];
        _useActionText_2_1 = text[4];
        _useActionText_2_2 = text[5];

        Image[] image = GetComponentsInChildren<Image>(true);
        _infoActionBg = image[0];
        _infoActionE_Bg = image[1];
        _infoActionE = image[2];
        _pickUpActionBg = image[3];
        _unablePickUpActionBg = image[4];
        _useActionBg_1 = image[5];
        _useActionBg_2_1 = image[6];
        _useActionBg_2_2 = image[7];
    }

    // 액션 텍스트 활성화 & 비활성화
    // 아이템 획득 가능 알림 문구를 보여주는 메서드
    public void ShowInfoActionText(bool isActive)
    {
        _infoActionText.gameObject.SetActive(isActive);
        _infoActionBg.gameObject.SetActive(isActive);
        _infoActionE_Bg.gameObject.SetActive(isActive);
        _infoActionE.gameObject.SetActive(isActive);
    }

    // 아이템 획득 알림 문구를 보여주는 메서드
    public void OnPickUpActionText()
    {
        // 인보크 왜 사용하는지 이해하기
        CancelInvoke("OffPickUpActionText");
        _pickUpActionText.gameObject.SetActive(true);
        _pickUpActionBg.transform.gameObject.SetActive(true);
        Invoke("OffPickUpActionText", 1f);
    }


    public void OffPickUpActionText()
    {
        _pickUpActionText.gameObject.SetActive(false);
        _pickUpActionBg.gameObject.SetActive(false);
    }

    // 아이템 획득 불가 문구 보여주는 메서드
    public void OnUnablePickUpActionText()
    {
        CancelInvoke("OffUnablePickUpActionText");
        _unablePickUpActionText.gameObject.SetActive(true);
        _unablePickUpActionBg.transform.gameObject.SetActive(true);
        Invoke("OffUnablePickUpActionText", 1.5f);
    }

    public void OffUnablePickUpActionText()
    {
        _unablePickUpActionText.gameObject.SetActive(false);
        _unablePickUpActionBg.transform.gameObject.SetActive(false);
    }

    // 아이템 사용 알림 문구를 한개 보여주는 메서드
    public void OnUseActionText_1()
    {
        CancelInvoke("OffUseActionText_1");
        _useActionText_1.gameObject.SetActive(true);
        _useActionBg_1.gameObject.SetActive(true);
        Invoke("OffUseActionText_1", 1.5f);
    }

    public void OffUseActionText_1()
    {
        _useActionText_1.gameObject.SetActive(false);
        _useActionBg_1.gameObject.SetActive(false);
    }

    // 아이템 사용 알림 문구를 두개 보여주는 메서드
    public void OnUseActionText_2()
    {
        CancelInvoke("OffUseActionText_2");
        _useActionText_2_1.gameObject.SetActive(true);
        _useActionText_2_2.gameObject.SetActive(true);
        _useActionBg_2_1.gameObject.SetActive(true);
        _useActionBg_2_2.gameObject.SetActive(true);
        Invoke("OffUseActionText_2", 1.5f);
    }

    public void OffUseActionText_2()
    {
        _useActionText_2_1.gameObject.SetActive(false);
        _useActionText_2_2.gameObject.SetActive(false);
        _useActionBg_2_1.gameObject.SetActive(false);
        _useActionBg_2_2.gameObject.SetActive(false);
    }


    // 액션 텍스트 바꾸기
    // 아이템 획득 가능 알림 문구 바꾸기
    public void ChangeInfoActionText(string text)
    {
        _infoActionText.text = text;
    }

    // 아이템 획득 불가 알림 문구 바꾸기
    public void ChangeUnablePickUpActionText(string text)
    {
        _unablePickUpActionText.text = text;
    }

    // 아이템 획득 알림 문구 바꾸기
    public void ChangePickUpActionText(string text)
    {
        _pickUpActionText.text = text;
    }

    // 아이템 사용 알림 문구 한개 바꾸기
    public void ChangeUseActionText_1(string text)
    {
        _useActionText_1.text = text;
    }

    // 아이템 사용 알림 문구 두개 바꾸기
    public void ChangeUseActionText_2(string text_2_1, string text_2_2)
    {
        _useActionText_2_1.text = text_2_1;
        _useActionText_2_2.text = text_2_2;
    }

    // 아이템 문구 저장
    // 아이템 획득 가능 알림 문구
    public void InfoActionText(Item item)
    {
        // Q: 예외처리 여기서도 해야하나요?
        if (item != null)
        {
            ChangeInfoActionText("Pick up the " + "<color=#F3DD95>" + item.itemName + "</color>");
        }
    }

    // 아이템 획득 불가 알림 문구
    public void UnablePickUpActionText(Item item)
    {
        if (item != null)
        {
            ChangeUnablePickUpActionText("The inventory is " + "<color=red>" + "full " + "</color>" + "and cannot be put in.");
        }
    }

    // 아이템 획득 알림 문구
    public void PickUpActionText(Item item)
    {
        if (item != null)
        {
            ChangePickUpActionText("Pick up an " + "<color=#11CAD6>" + item.itemName + "</color>");
        }
    }


    // 아이템 사용 알림 문구
    public void UseActionText(Item item)
    {
        if (item.itemType == ItemType.Consumable)
        {
            switch (item.ammoType)
            {
                // 즉시 사용 아이템 Sniper & RifleAmmoPack
                case AmmoType.Rifle:
                    ChangeUseActionText_2("<color=#F3DD95>" + item.itemName + "</color>" + " Successful use ", "<color=#11CAD6>" + "Rifle Bullets +" + item.rifleAmmoValue + "</color>");
                    break;
                case AmmoType.Sniper:
                    ChangeUseActionText_2("<color=#F3DD95>" + item.itemName + "</color>" + " Successful use ", "<color=#11CAD6>" + "Sniper Bullets +" + item.sniperAmmoValue + "</color>");
                    break;
                // TotalAmmoPack
                case AmmoType.Total:
                    ChangeUseActionText_2("<color=#F3DD95>" + item.itemName + "</color>" + " Successful use ", "<color=#11CAD6>" + "Sniper Bullets +" + item.sniperAmmoValue + "  , Rifle Bullets +" + item.rifleAmmoValue + "</color>");
                    break;
                default:
                    ChangeUseActionText_1("Data is not set");
                    Debug.Log("Data is not set");
                    break;
            }
        }
        else
        {
            // Health, Stamina, Quest
            switch (item.itemType)
            {
                case ItemType.Health:
                    ChangeUseActionText_2("<color=#F3DD95>" + item.itemName + "</color>" + " Successful use ", "<color=#11CAD6>" + "Health +" + item.healthValue + "  , Stamina +" + item.staminaValue + "</color>");
                    break;
                case ItemType.Stamina:
                    ChangeUseActionText_2("<color=#F3DD95>" + item.itemName + "</color>" + " Successful use ", "<color=#11CAD6>" + "Health +" + item.healthValue + "  , Stamina +" + item.staminaValue + "</color>");
                    break;
                default:
                    // 아이템 값 증가로 바꿀 경우 수정 할 것
                    ChangeUseActionText_1("Data is not set");
                    break;
            }
        }
    }
}
