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

    private CancellationTokenSource cts;

    public DefaultSkill(int skillCost, Light directionalLight, float duration = 5f, float targetIntensity = 2f)
    {
        this.skillCost = skillCost;
        this.directionalLight = directionalLight;
        this.duration = duration;
        this.targetIntensity = targetIntensity;
    }

    public override int GetSkillCost()
    {
        return base.GetSkillCost();
    }

    public override bool CanActivate()
    {
        return base.CanActivate();
    }

    public override void Activate()
    {
        cts?.Cancel(); // ëΩèdî≠ìÆëŒçÙ
        cts = new CancellationTokenSource();
        LightUpAsync(cts.Token).Forget();
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
