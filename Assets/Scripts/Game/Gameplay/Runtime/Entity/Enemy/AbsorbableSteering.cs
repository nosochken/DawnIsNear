using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class AbsorbableSteering
    {
        private float _absorbRadius;
        private float _epsilon;
        
        internal AbsorbableSteering(float absorbRadius, float epsilon)
        {
            _absorbRadius = absorbRadius;
            _epsilon = epsilon;
        }
        
        internal Vector2 ComputeAbsorbablesAttraction(IReadOnlyCollection<IAbsorbable> absorbables, Vector2 selfPosition)
        {
            float absorbableRadiusSqr = _absorbRadius * _absorbRadius;
            Vector2 sum = Vector2.zero;

            foreach (IAbsorbable absorbable in absorbables)
            {
                if (absorbable == null || !absorbable.IsActive) continue;

                Vector2 toAbsorbable = absorbable.CurrentPosition - selfPosition;
                float sqrAbsorbableDistance = toAbsorbable.sqrMagnitude;
                
                if (sqrAbsorbableDistance < SteeringMath.MinSqrMagnitudeForDirection || sqrAbsorbableDistance > absorbableRadiusSqr) continue;

                float distanceWeight = 1f / (sqrAbsorbableDistance + _epsilon);
                sum += toAbsorbable.normalized * distanceWeight;
            }

            return sum;
        }
    }
}