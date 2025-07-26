using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;
using System;

public class HiddenSkill : SkillBase
{
    private readonly float duration = 5f;

    private CancellationTokenSource cts;
    private PlayerManager playerManager;
    private PlayerLocomotionManager playerLocomotionManager;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material hiddenMaterial;

    public HiddenSkill(int skillCost,
        PlayerManager playerManager,
        SkinnedMeshRenderer skinnedMeshRenderer,
        Material hiddenMaterial,
        PlayerLocomotionManager playerLocomotionManager,
        float duration)
    {
        this.skillCost = skillCost;
        this.playerManager = playerManager;
        this.playerLocomotionManager = playerLocomotionManager;
        this.skinnedMeshRenderer = skinnedMeshRenderer;
        this.hiddenMaterial = hiddenMaterial;
        this.duration = duration;
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
        cts?.Cancel();
        cts = new CancellationTokenSource();

        HiddenGhostAsync(cts.Token).Forget();
    }

    private async UniTaskVoid HiddenGhostAsync(CancellationToken token)
    {
        isRunning = true;

        // 元のマテリアルを保存
        Material originalMaterial = skinnedMeshRenderer.material;
        float originalSpeed = playerLocomotionManager.MoveSpeed;

        skinnedMeshRenderer.material = hiddenMaterial;
        playerManager.SetIsHidden(true);
        playerLocomotionManager.SetMoveSpeed(originalSpeed * 1.5f);

        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: token);
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            // 元のマテリアルに戻す
            skinnedMeshRenderer.material = originalMaterial;
            playerManager.SetIsHidden(false);
            playerLocomotionManager.SetMoveSpeed(originalSpeed);
            isRunning = false;
        }
    }

    public void Cancel()
    {
        cts?.Cancel();
    }
}
