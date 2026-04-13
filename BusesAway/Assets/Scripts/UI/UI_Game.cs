using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    private Canvas canvas;

    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image progressImage;
    private float targetFill;

    private void Awake()
    {
        this.canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        this.canvas.enabled = true;
    }

    public void Hide()
    {
        this.canvas.enabled = false;
    }

    public void SetProgress(int current, int total)
    {
        this.targetFill = current * 1.0f / total;
        this.progressText.text = $"{total}";
    }

    private void Update()
    {
        this.progressImage.fillAmount = Mathf.MoveTowards(this.progressImage.fillAmount, this.targetFill, 3f * Time.fixedDeltaTime);
    }
}
