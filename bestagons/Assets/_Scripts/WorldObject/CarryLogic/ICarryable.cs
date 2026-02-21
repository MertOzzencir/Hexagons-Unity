
using UnityEngine;

public interface ICarryable
{
    public void OnPicked();
    public void Carry(Vector3 toPosition);
    public void Drop();
    public void LocomotionLogic();
    public void TryToPlaceOn();

}
