using System;
using UnityEngine;

namespace Game.Gameplay
{
    public interface ISpawnable<T> where T : MonoBehaviour, ISpawnable<T>
    {
       public event Action<T> ReadyToSpawn;
    }
}