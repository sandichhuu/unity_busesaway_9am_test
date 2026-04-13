using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Win : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        this.canvas = GetComponent<Canvas>();
        Hide();
    }

    public void Show()
    {
        this.canvas.enabled = true;
    }

    public void Hide()
    {
        this.canvas.enabled = false;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
