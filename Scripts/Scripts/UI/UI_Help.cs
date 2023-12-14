using UnityEngine;

public class UI_Help : MonoBehaviour
{
    public GameObject helpUI; // ����Ű â�� UI GameObject
    private bool isHelpOpen = false; // ����Ű â�� �����ִ��� ����

    // ����Ű â ����/�ݱ� ��ư�� ������ �� ȣ��� �Լ�
    public void ToggleHelp()
    {
        isHelpOpen = !isHelpOpen; // ���¸� ���

        // ����Ű â�� ���ų� ����
        if (isHelpOpen)
        {
            OpenHelp();
        }
        else
        {
            CloseHelp();
        }
    }

    // ����Ű â�� ����
    void OpenHelp()
    {
        helpUI.SetActive(true); // ����Ű â�� Ȱ��ȭ
        // �ٸ� �ʿ��� �ʱ�ȭ �۾� ����
    }

    // ����Ű â�� �ݱ�
    void CloseHelp()
    {
        helpUI.SetActive(false); // ����Ű â�� ��Ȱ��ȭ
        // �ٸ� �ʿ��� ���� �۾� ����
    }

    // X ��ư�� ������ �� ȣ��� �Լ�
    public void CloseHelpButton()
    {
        if (isHelpOpen)
        {
            CloseHelp();
        }
    }
}
