using UnityEngine;

public class EntityBase : MonoBehaviour
{
    protected bool gameStart = false;

    public virtual void GameStart()
    {
        gameStart = true;
    }
}
