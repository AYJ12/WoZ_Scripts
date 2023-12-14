using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotTooltip : UI_PopUp
{
    [SerializeField]
    private GameObject _goBase;
    private Text _textItemName;
    private Text _textItemContent;
    private Text _textHealthValue;
    private Text _textStaminaValue;
    private Text _textSniperAmmoValue;
    private Text _textRifleAmmoValue;
    private Text _textTotal_SniperAmmoValue;
    private Text _textTotal_RifleAmmoValue;

    private void Awake()
    {
        GameObject baseFrontB = _goBase.transform.GetChild(0).gameObject;
        _textItemName = baseFrontB.transform.GetChild(0).GetComponent<Text>();

        GameObject baseFrontF = _goBase.transform.GetChild(1).gameObject;
        _textItemContent = baseFrontF.transform.GetChild(0).GetComponent<Text>();
        _textHealthValue = baseFrontF.transform.GetChild(1).GetComponent<Text>();
        _textStaminaValue = baseFrontF.transform.GetChild(2).GetComponent<Text>();
        _textSniperAmmoValue = baseFrontF.transform.GetChild(3).GetComponent<Text>();
        _textRifleAmmoValue = baseFrontF.transform.GetChild(4).GetComponent<Text>();
        _textTotal_SniperAmmoValue = baseFrontF.transform.GetChild(5).GetComponent<Text>();
        _textTotal_RifleAmmoValue = baseFrontF.transform.GetChild(6).GetComponent<Text>();
    }
    public void ShowTooltip(Item _item, Vector3 _pos)
    {
        _pos += new Vector3(_goBase.GetComponent<RectTransform>().rect.width * 0.57f, -_goBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        _goBase.transform.position = _pos;

        _textItemName.text = _item.itemName;
        _textItemContent.text = _item.itemContent;

        // AmmoPack 아이템
        if (_item.itemType == ItemType.Consumable)
        {
            switch (_item.ammoType)
            {
                case AmmoType.Sniper:
                    _textSniperAmmoValue.text = "<color=#11CAD6>" + "Sniper Bullet : " + "</color>" + "<color=#000000>" + " + " + _item.sniperAmmoValue + "</color>";
                    ShowTooltipProPerty(_item);
                    break;
                case AmmoType.Rifle:
                    _textRifleAmmoValue.text = "<color=#11CAD6>" + "Rifle Bullet : " + "</color>" + "<color=#000000>" + " + " + _item.rifleAmmoValue + "</color>";
                    ShowTooltipProPerty(_item);
                    break;
                case AmmoType.Total:
                    _textTotal_SniperAmmoValue.text = "<color=#5B8F58>" + "Sniper : " + "</color>" + "<color=#000000>" + "<b>" + " + " + _item.sniperAmmoValue + "</b>" + "</color>";
                    _textTotal_RifleAmmoValue.text = "<color=#5B8F58>" + "Rifle : " + "</color>" + "<color=#000000>" + "<b>" + " + " + _item.rifleAmmoValue + "</b>" + "</color>";
                    ShowTooltipProPerty(_item);
                    break;
                default:
                    Debug.Log("지정된 값이 없는 데이터입니다.");
                    break;
            }
        }
        // Health, Stamina 아이템
        else if (_item.itemType == ItemType.Health || _item.itemType == ItemType.Stamina)
        {
            _textHealthValue.text = "<color=#EA7446>" + "Health: " + "</color>" + "<color=#000000>" + " +" + _item.healthValue + "</color>";
            _textStaminaValue.text = "<color=#5192EC>" + "Stamina : " + "</color>" + "<color=#000000>" + " +" + _item.staminaValue + "</color>";
            ShowTooltipProPerty(_item);
        }
        else
        {
            // 퀘스트 아이템 
        }
    }

    private void ShowTooltipProPerty(Item _item)
    {
        if (_item.itemType == ItemType.Consumable)
        {
            switch (_item.ammoType)
            {
                case AmmoType.Sniper:
                    _textSniperAmmoValue.gameObject.SetActive(true);
                    break;
                case AmmoType.Rifle:
                    _textRifleAmmoValue.gameObject.SetActive(true);
                    break;
                case AmmoType.Total:
                    _textTotal_SniperAmmoValue.gameObject.SetActive(true);
                    _textTotal_RifleAmmoValue.gameObject.SetActive(true);
                    break;
                default:
                    Debug.Log("지정된 값이 없는 데이터입니다.");
                    break;
            }
        }
        // Health, Stamina 아이템
        else if (_item.itemType == ItemType.Health || _item.itemType == ItemType.Stamina)
        {
            _textHealthValue.gameObject.SetActive(true);
            _textStaminaValue.gameObject.SetActive(true);
        }
        else
        {
            // 퀘스트 아이템 
        }
    }

    private void HideTooltipProperty()
    {
        _textSniperAmmoValue.gameObject.SetActive(false);
        _textRifleAmmoValue.gameObject.SetActive(false);
        _textTotal_SniperAmmoValue.gameObject.SetActive(false);
        _textTotal_RifleAmmoValue.gameObject.SetActive(false);
        _textHealthValue.gameObject.SetActive(false);
        _textStaminaValue.gameObject.SetActive(false);
        //if (_item.itemType == ItemType.Consumable)
        //{
        //    switch (_item.ammoType)
        //    {
        //        case AmmoType.Sniper:
        //            break;
        //        case AmmoType.Rifle:
        //            break;
        //        case AmmoType.Total:
        //            break;
        //        default:
        //            Debug.Log("지정된 값이 없는 데이터입니다.");
        //            break;
        //    }
        //}
        //// Health, Stamina 아이템
        //else if (_item.itemType == ItemType.Health || _item.itemType == ItemType.Stamina)
        //{
        //}
        //else
        //{
        //    // 퀘스트 아이템 
        //}
    }

    // 각 아이템의 속성 값 끄기
    public void CloseTooltipProperty()
    {
        HideTooltipProperty();
    }
}
