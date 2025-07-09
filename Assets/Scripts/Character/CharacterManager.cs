using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] protected float hp;

    public float GetHp()
    {
        return hp;
    }
}
