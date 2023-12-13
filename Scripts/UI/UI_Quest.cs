using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : MonoBehaviour
{
    private UI_GameScene _uiGameScene;
    private Text _questText; // UI Text ������Ʈ�� ����ų ����
    private int _currentQuestIndex = 0; // ���� �̼� �ε����� ��Ÿ���� ����
    private string[] questTexts = { "Find the doctor!", "Go to the government base!" }; // �̼� �ؽ�Ʈ �迭
    private NpcInteractAI _npcInteractAI;

    private void Start()
    {
        _uiGameScene = GetComponent<UI_GameScene>();
        _questText = _uiGameScene.GetText((int)UI_GameScene.Texts.QuestText);
        _npcInteractAI = NpcObject.Instance.GetComponent<NpcInteractAI>();
        DisplayQuestText(_currentQuestIndex); // ���� �� ù ��° �̼� �ؽ�Ʈ�� ǥ��
    }

    private void Update()
    {
        if (_npcInteractAI.isActive)
        {
            // E Ű�� ������ ��, �̼��� ���� �ܰ�� ����
            _currentQuestIndex++;
            if (_currentQuestIndex < questTexts.Length)
            {
                DisplayQuestText(_currentQuestIndex);
            }
        }
    }

    void DisplayQuestText(int index)
    {
        if (index < questTexts.Length)
        {
            _questText.text = questTexts[index];
        }
        else
        {
            _questText.text = "�̼� �Ϸ�"; // ��� �̼��� �Ϸ��� ���
        }
    }
}
