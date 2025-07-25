using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class BlinkingTextUniTask : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float blinkInterval = 0.5f; // 点滅間隔（秒）

    private bool isBlinking = false;
    private Color originalColor;

    private void OnEnable()
    {
        if (targetText == null)
        {
            Debug.LogWarning("TextMeshProUGUI が設定されていません。");
            return;
        }

        originalColor = targetText.color;
        isBlinking = true;
        StartBlinkingAsync().Forget();
    }

    private void OnDisable()
    {
        isBlinking = false;
        if (targetText != null)
        {
            SetTextAlpha(1f); // 元に戻す
        }
    }

    private async UniTaskVoid StartBlinkingAsync()
    {
        bool isVisible = true;

        while (isBlinking)
        {
            isVisible = !isVisible;
            SetTextAlpha(isVisible ? 1f : 0f);
            await UniTask.Delay((int)(blinkInterval * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        }

        SetTextAlpha(1f);
    }

    private void SetTextAlpha(float alpha)
    {
        if (targetText != null)
        {
            Color color = originalColor;
            color.a = alpha;
            targetText.color = color;
        }
    }
}
