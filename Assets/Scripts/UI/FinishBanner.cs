using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class FinishBanner : MonoBehaviour
{
    [Header("ゲームデータの参照")]
    [SerializeField] private GameStateData gameStateData;

    [Header("UI参照")]
    [SerializeField] private RectTransform bannerTransform;

    [Header("表示位置")]
    [SerializeField] private Vector2 leftOffScreen = new Vector2(-1920, 0);
    [SerializeField] private Vector2 centerScreen = new Vector2(0, 0);
    [SerializeField] private Vector2 rightOffScreen = new Vector2(1920, 0);

    [Header("アニメーション設定")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float visibleDuration = 2f;

    [Header("クリア後のシーン名")]
    [SerializeField] private string gameOverSceneName = "";

    public void Setup()
    {
        // 最初は画面左外に置いておく
        bannerTransform.anchoredPosition = leftOffScreen;
    }

    public async UniTaskVoid ShowFinishBannerAsync(bool isCleared)
    {
        gameStateData.SetIsCleared(isCleared);

        // スライドイン（左 → 中央）
        await SlideAsync(from: leftOffScreen, to: centerScreen);

        // 一定時間表示
        await UniTask.Delay(System.TimeSpan.FromSeconds(visibleDuration));

        // スライドアウト（中央 → 右）
        await SlideAsync(from: centerScreen, to: rightOffScreen);

        await SceneManager.LoadSceneAsync(gameOverSceneName);
    }

    private async UniTask SlideAsync(Vector2 from, Vector2 to)
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            bannerTransform.anchoredPosition = Vector2.Lerp(from, to, t);
            await UniTask.Yield();
        }

        bannerTransform.anchoredPosition = to;
    }
}
