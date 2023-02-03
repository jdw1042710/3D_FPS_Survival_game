using UnityEngine;

public abstract class FrendlyAnimal : Animal
{
    
    
    public override void Damage(int _damage, Vector3 _targetPosition)
    {
        base.Damage(_damage, _targetPosition);
        
        ResetAll();
        Run(_targetPosition);
    }
    
    public void Run(Vector3 _targetPosition)
    {
        isRunning = true;
        int id = Animator.StringToHash("Run");
        animator.SetBool(id, isRunning);

        navMeshAgent.speed = runSpeed;
        destination = new Vector3(
            transform.position.x - _targetPosition.x,
            0f,
            transform.position.z - _targetPosition.z
        ).normalized;

        currentTime = runtime;
        
    }
}
