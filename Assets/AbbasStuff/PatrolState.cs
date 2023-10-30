using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
  public override void Enter()
  {
    Debug.Log("Entering Patrol State");
  }

  public override void Execute()
  {
    Patrol();
    Debug.Log("Executing Patrol State");
  }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }

    public void Patrol()
    {
        if (ghost.Agent.remainingDistance < 0.2f)
        {
            if (waypointIndex < ghost.patrolPath.patrolPoints.Count - 1)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
            ghost.Agent.SetDestination(ghost.patrolPath.patrolPoints[waypointIndex].position);
        }
        Debug.Log("Patrolling");

    }
}
