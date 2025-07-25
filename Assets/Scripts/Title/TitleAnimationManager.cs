using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleAnimationManager : MonoBehaviour
{
    [Header("演出ターゲット")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform logoImageTransform;
    [SerializeField] private GameObject logoObject;
    [SerializeField] private GameObject recommendTextUI;

    [Header("移動演出の設定")]
    [SerializeField] private Vector3 pushPosition = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 targetScale = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] private Vector3 pushOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 hiddenLogoPosition = new Vector3(0, 1500, 6000);
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float hideDuration = 1.5f;
    [SerializeField] private float attackStartDelay = 0.5f;
    [SerializeField] private float attackEndDelay = 0.5f;
    [SerializeField] private float postAttackDelay = 1f;
    [SerializeField] private float logoSlideHeight = 5f;

    private Vector3 logoStartPosition;
    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;
    private Vector3 initialPlayerScale;

    private void Awake()
    {
        logoStartPosition = logoImageTransform.localPosition;
        initialPlayerPosition = playerTransform.position;
        initialPlayerRotation = playerTransform.rotation;
        initialPlayerScale = playerTransform.localScale;
    }

    public async UniTask PlayExitSequenceAsync()
    {
        recommendTextUI.SetActive(false);

        // スライドアウト
        Vector3 currentPos = playerTransform.position;
        Vector3 slideOutPos = currentPos + Vector3.up * logoSlideHeight;
        await SlideAsync(playerTransform, currentPos, slideOutPos, slideDuration);

        // 中間点
        Vector3 midPos = new Vector3(pushPosition.x, slideOutPos.y, slideOutPos.z);
        await SlideAsync(playerTransform, slideOutPos, midPos, slideDuration / 2f);

        // スライドイン
        Vector3 slideInTarget = pushPosition + pushOffset;
        await SlideAsync(playerTransform, midPos, slideInTarget, slideDuration);

        // ロゴをフェードアウト＋プレイヤー縮小
        await HideLogoAndShrinkPlayerAsync();
    }

    private async UniTask HideLogoAndShrinkPlayerAsync()
    {
        float elapsed = 0f;
        while (elapsed < hideDuration)
        {
            float t = elapsed / hideDuration;

            logoImageTransform.localPosition = Vector3.Lerp(logoStartPosition, hiddenLogoPosition, t);
            playerTransform.localScale = Vector3.Lerp(initialPlayerScale, targetScale, t);
            playerTransform.position = Vector3.Lerp(pushPosition, pushPosition + pushOffset, t);

            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }

        logoImageTransform.localPosition = hiddenLogoPosition;
        playerTransform.localScale = targetScale;
        playerTransform.position = pushPosition + pushOffset;

        await TriggerAttackAndHideAsync();
    }

    private async UniTask TriggerAttackAndHideAsync()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(attackStartDelay));
        animator.SetTrigger("Attack");
        await UniTask.Delay(System.TimeSpan.FromSeconds(attackEndDelay));

        if (playerObject != null) playerObject.SetActive(false);
        if (logoObject != null) logoObject.SetActive(false);

        await UniTask.Delay(System.TimeSpan.FromSeconds(postAttackDelay));

        // 後処理：プレイヤーを初期状態に戻す（次回に備える）
        playerObject.SetActive(true);
        playerTransform.position = initialPlayerPosition;
        playerTransform.rotation = initialPlayerRotation;
        playerTransform.localScale = initialPlayerScale;
    }

    private async UniTask SlideAsync(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.position = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        target.position = to;
    }
}
