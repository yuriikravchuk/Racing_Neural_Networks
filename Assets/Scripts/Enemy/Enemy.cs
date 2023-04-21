using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAI _ai;

    public void Init(EnemyAI ai) => _ai = ai;

    private void FixedUpdate()
    {
        //Vector2 moveVector = _ai.GetMoveVector();

        //_movement.ApplyInputs(moveVector.x, moveVector.y);
    }
}
