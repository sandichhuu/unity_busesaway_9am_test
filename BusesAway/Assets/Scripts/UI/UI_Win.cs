using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Win : MonoBehaviour
{
    private Canvas canvas;
    private CanvasGroup group;

    private void Awake()
    {
        this.canvas = GetComponent<Canvas>();
        this.group = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Show()
    {
        this.canvas.enabled = true;
        AnimateShow().Forget();
    }

    private async UniTask AnimateShow()
    {
        this.group.DOFade(1f, 1f);
    }

    public void Hide()
    {
        this.canvas.enabled = false;
        this.group.alpha = 0f;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
