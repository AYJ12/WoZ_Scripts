using UnityEngine;

public class UI_Help : MonoBehaviour
{
    public GameObject helpUI; // 조작키 창의 UI GameObject
    private bool isHelpOpen = false; // 조작키 창이 열려있는지 여부

    // 조작키 창 열기/닫기 버튼을 눌렀을 때 호출될 함수
    public void ToggleHelp()
    {
        isHelpOpen = !isHelpOpen; // 상태를 토글

        // 조작키 창을 열거나 닫음
        if (isHelpOpen)
        {
            OpenHelp();
        }
        else
        {
            CloseHelp();
        }
    }

    // 조작키 창을 열기
    void OpenHelp()
    {
        helpUI.SetActive(true); // 조작키 창을 활성화
        // 다른 필요한 초기화 작업 수행
    }

    // 조작키 창을 닫기
    void CloseHelp()
    {
        helpUI.SetActive(false); // 조작키 창을 비활성화
        // 다른 필요한 정리 작업 수행
    }

    // X 버튼을 눌렀을 때 호출될 함수
    public void CloseHelpButton()
    {
        if (isHelpOpen)
        {
            CloseHelp();
        }
    }
}
