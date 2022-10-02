using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadScene("InGame");
    }
}