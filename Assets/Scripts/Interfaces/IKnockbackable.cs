using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(float force, float radius, Vector3 pos);
}