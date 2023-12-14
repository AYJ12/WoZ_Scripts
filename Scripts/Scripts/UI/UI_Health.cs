using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IDamageable
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable]
public class UI_Health : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public float regenRate;
    public float decayRate;

    private UI_GameScene _uiGameScene;
    private Image _hpImage;
    private Image _staminaImage;
    private PlayerStatHandler _playerStatHandler;
    private float _hp;
    private float _stamina;
    public UnityEvent onTakeDamage;

    private void Start()
    {
        _playerStatHandler = Player.Instance.playerStatHandler;
        _uiGameScene = GetComponent<UI_GameScene>();
        // component���� ã�� �Ҵ�
        _hpImage = _uiGameScene.GetImage((int)UI_GameScene.Images.HpBar);
        _staminaImage = _uiGameScene.GetImage((int)UI_GameScene.Images.StaminaBar);

        //Canvas canvas = FindObjectOfType<UI_GameScene>().GetComponent<Canvas>();    // ���� ������Ʈ�� ���� �� ��ġ��
        //if (canvas != null)
        //{
        //    GameObject playerCondition = canvas.transform.Find("Conditions").gameObject;
        //    hpUiBar = playerCondition.transform.Find("HP").gameObject.transform.Find("HpBar").gameObject;
        //    staminaUiBar = playerCondition.transform.Find("Stamina").gameObject.transform.Find("StaminaBar").gameObject;
        //}
        _hp = _playerStatHandler.currentStat.playerHp;
        _stamina = _playerStatHandler.currentStat.playerStamina;
    }

    private void Update()
    {
        _hp = _playerStatHandler.currentStat.playerHp;
        _stamina = _playerStatHandler.currentStat.playerStamina;

        //stamina.Add(stamina.regenRate * Time.deltaTime); ���¹̳� ȸ��

        //if (_hp == 0.0f)
        //    Die();
        _hpImage.fillAmount = GetPercentage(_hp, _playerStatHandler.playerMaxHp);
        _staminaImage.fillAmount = GetPercentage(_stamina, _playerStatHandler.playerMaxStamina);

    }

    public float GetPercentage(float curValue, float maxValue)
    {
        return curValue / maxValue;
    }


    public void Die()
    {
        Debug.Log("�÷��̾ �׾����ϴ�");   //TODO ����ȯ or UI ����
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        onTakeDamage?.Invoke();
    }
}

