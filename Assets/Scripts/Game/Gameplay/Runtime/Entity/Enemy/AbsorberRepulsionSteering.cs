using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class AbsorberRepulsionSteering
    {
        private float _dangerRadius;
        private float _minRepulsionBoost;
        private float _maxRepulsionBoost;
        private float _epsilon;
        
        internal AbsorberRepulsionSteering(float dangerRadius, float minRepulsionBoost, float maxRepulsionBoost, float epsilon)
        {
            _dangerRadius = dangerRadius;
            _minRepulsionBoost = minRepulsionBoost;
            _maxRepulsionBoost = maxRepulsionBoost;
            _epsilon = epsilon;
        }
        
        internal Vector2 Compute(out float panic, IReadOnlyCollection<IAbsorber> absorbers, IAbsorbable self)
        {
            float dangerRadiusSqr = _dangerRadius * _dangerRadius;
            Vector2 sum = Vector2.zero;
            float maxPanic = 0f;

            foreach (IAbsorber absorber in absorbers)
            {
                if (absorber == null || !absorber.Body.IsActive) continue;
                if (!absorber.CanAbsorb(self)) continue;

                Vector2 away = self.Body.CurrentPosition - absorber.Body.CurrentPosition;
                float sqrAwayDistance = away.sqrMagnitude;
                if (sqrAwayDistance < DirectionMath.MinSqrMagnitudeForDirection || sqrAwayDistance > dangerRadiusSqr) continue;

                float awayDistance = Mathf.Sqrt(sqrAwayDistance);
                float currentPanic = 1f - (awayDistance / _dangerRadius);
                
                if (currentPanic > maxPanic) 
                    maxPanic = currentPanic;

                float distanceWeight = 1f / (sqrAwayDistance + _epsilon);

                float sizeRatio = (absorber.Body.Size.Current - self.Body.Size.Current) / (float)Mathf.Max(self.Body.Size.Current, 1);
                float sizeBoost = Mathf.Lerp(_minRepulsionBoost, _maxRepulsionBoost, Mathf.Clamp01(sizeRatio));

                sum += away.normalized * distanceWeight * sizeBoost;
            }

            panic = maxPanic;
            return sum;
        }
    }
}