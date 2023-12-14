using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameScene : UI_Base
{
    public enum GameObjects
    {
        MiniMap,
        ShowText,
        Scope
    }
    public enum TextMeshs
    {
        WeaponName,
        WeaponAmmo,
        Prompt,
    }
    public enum Texts
    {
        QuestText,
    }

    public enum Images
    {
        HpBar,
        StaminaBar,
        WeaponIcon,
        DamageIndicator,
        PromptBG,
    }
    public override void Init()
    {
        Manager.UI.SetCanvas(gameObject, false);
    }

    private void Awake()
    {
        Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(TextMeshs));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }
}
