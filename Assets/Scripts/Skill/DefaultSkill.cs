using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DefaultSkill : SkillBase
{
    private readonly Light directionalLight;
    private readonly float duration;
    private readonly float targetIntensity;
    private readonly float originalIntensity = 0f;
    private readonly int skillAmount = 1;

    private CancellationTokenSource cts;
    private bool isRunning = false;

    public DefaultSkill(Light directionalLight, float duration = 5f, float targetIntensity = 2f)
    {
        this.directionalLight = directionalLight;
        this.duration = duration;
        this.targetIntensity = targetIntensity;
    }

    public override int Activate()
    {
        if (isRunning) return 0;

        cts?.Cancel(); // ëΩèdî≠ìÆëŒçÙ
        cts = new CancellationTokenSource();
        LightUpAsync(cts.Token).Forget();
        return skillAmount;
    }

    private async UniTaskVoid LightUpAsync(CancellationToken token)
    {
        if (directionalLight == null)
        {
            Debug.LogWarning("DirectionalLightÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
            return;
        }

        isRunning = true;

        directionalLight.intensity = targetIntensity;

        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            // ÉLÉÉÉìÉZÉãéûÇ‡å≥Ç…ñﬂÇ∑
        }
        finally
        {
            directionalLight.intensity = originalIntensity;
            isRunning = false;
        }
    }

    public void Cancel()
    {
        cts?.Cancel();
    }
}
