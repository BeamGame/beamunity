using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {

            var instance = GameController.Instance;
            if (instance.CanBattle())
            {
                player.Character.Animator.IsMoving = false;
                instance.StartBattle(BattleTrigger.LongGrass);
            }
        }
    }

    public bool TriggerRepeatedly => true;
}
