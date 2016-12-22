using UnityEngine;
using System.Collections;

public interface Projectile {
    void Shoot(GameObject owner, Vector2 position, Vector2 target, float speedScale);
}
