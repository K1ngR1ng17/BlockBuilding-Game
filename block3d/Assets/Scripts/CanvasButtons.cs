using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour  
{
    public Sprite musicOn, musicOff;
    private void Start()
    {
        if (PlayerPrefs.GetString("Music") == "No" && gameObject.name == "Volume")
            GetComponent<Image>().sprite = musicOff;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadInstagram()
    {
        Application.OpenURL("https://www.instagram.com/k1ng_r1ng/");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void CloseShop()
    {
        SceneManager.LoadScene("Main");
    }
    public void musicWork()
    {
        if (PlayerPrefs.GetString("Music") == "No")
        {
            PlayerPrefs.SetString("Music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("Music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
