using UnityEngine;

public abstract class FrendlyAnimal : Animal
{
    
    
    public void Damage(int _damage, Vector3 _targetPosition)
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

        applySpeed = runSpeed;
        direction = Quaternion.LookRotation(transform.position - _targetPosition).eulerAngles;
        
        currentTime = runtime;
        
    }
}
