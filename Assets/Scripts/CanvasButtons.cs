using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;

    public void Start()
    {
        if (PlayerPrefs.GetString("music") == "No"&& gameObject.name=="Music")
            GetComponent<Image>().sprite = musicOff;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadInstagram()
    {
        Application.OpenURL("https://www.instagram.com");
    }

    public void Music()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
