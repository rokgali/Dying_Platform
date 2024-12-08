using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarryable 
{
    List<IPickable> PickableObjects { get; set; }
    void Carry();
}
