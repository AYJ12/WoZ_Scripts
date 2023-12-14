using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public string EndScene;

    public void ExitGame()
    {
        SceneManager.LoadScene(EndScene);
    }
}
