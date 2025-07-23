using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleGhostAnimation : MonoBehaviour
{
    [Header("プレイヤーの参照")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Vector3 pushPosition = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 targetScale = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] private Vector3 pushOffset = new Vector3(0, 1.5f, 0);

    [Header("アニメーターの参照")]
    [SerializeField] private Animator animator;

    [Header("ロゴのアニメーションの設定")]
    [SerializeField] private Transform logoImageTransform;
    [SerializeField] private GameObject logoObject; // ロゴをまとめて消す場合はこちら
    [SerializeField] private Vector3 hiddenPosition = new Vector3(0, 1500, 6000);
    [SerializeField] private float hideDuration = 1.5f;

    [Header("時間の設定")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 7f;

    private Vector3 logoStartPosition;
    private Vector3 defaultPlayerScale;
    private float animationTimer;
    private float hideTimer = 0f;

    private bool isHidingLogo = false;
    private bool isAttackTriggered = false;

    private void Start()
    {
        ResetTimer();
        logoStartPosition = logoImageTransform.localPosition;
        defaultPlayerScale = playerTransform.localScale;
    }

    private void Update()
    {
        animationTimer -= Time.deltaTime;

        if (animationTimer <= 0f)
        {
            PlayRandomTrigger();
            ResetTimer();
        }

        if (!isHidingLogo && Input.anyKeyDown)
        {
            isHidingLogo = true;
            animator.SetTrigger("Cancel");
            hideTimer = 0f;
        }

        if (isHidingLogo && !isAttackTriggered)
        {
            hideTimer += Time.deltaTime;
            float t = Mathf.Clamp01(hideTimer / hideDuration);

            logoImageTransform.localPosition = Vector3.Lerp(logoStartPosition, hiddenPosition, t);
            Vector3 currentScale = Vector3.Lerp(defaultPlayerScale, targetScale, t);
            playerTransform.localScale = currentScale;

            Vector3 positionOffset = Vector3.Lerp(Vector3.zero, pushOffset, t);
            playerTransform.position = pushPosition + positionOffset;
            playerTransform.rotation = Quaternion.Euler(0, 0, 0);

            if (t >= 1f)
            {
                isAttackTriggered = true;
                TriggerAttackSequenceAsync().Forget();
            }
        }
    }

    private async UniTaskVoid TriggerAttackSequenceAsync()
    {
        // 1テンポ（0.5秒）待つ
        await UniTask.Delay(500);

        // Attackアニメーション再生
        animator.SetTrigger("Attack");

        await UniTask.Delay(500);

        // アニメ終了後にプレイヤーとロゴを非表示
        if (playerObject != null) playerObject.SetActive(false);
        if (logoObject != null) logoObject.SetActive(false);
    }

    private void ResetTimer()
    {
        animationTimer = Random.Range(minInterval, maxInterval);
    }

    private void PlayRandomTrigger()
    {
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("Surprised");
        }
    }
}
