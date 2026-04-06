using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class ThreatSteering
    {
        private float _dangerRadius;
        private float _minBoost;
        private float _maxThreatRepulsionBoost;
        private float _epsilon;
        
        internal ThreatSteering(float dangerRadius, float minBoost, float maxThreatRepulsionBoost, float epsilon)
        {
            _dangerRadius = dangerRadius;
            _minBoost = minBoost;
            _maxThreatRepulsionBoost = maxThreatRepulsionBoost;
            _epsilon = epsilon;
        }
        
        internal Vector2 ComputeThreatRepulsion(out float panic, IReadOnlyCollection<IBody> absorbers, 
            Vector2 selfPosition, int selfSize)
        {
            float dangerRadiusSqr = _dangerRadius * _dangerRadius;
            Vector2 sum = Vector2.zero;
            float maxPanic = 0f;

            foreach (IBody threat in absorbers)
            {
                if (threat == null || !threat.IsActive) continue;
                if (threat.Size.Current <= selfSize) continue;

                Vector2 away = selfPosition - threat.CurrentPosition;
                float sqrAwayDistance = away.sqrMagnitude;
                if (sqrAwayDistance < DirectionMath.MinSqrMagnitudeForDirection || sqrAwayDistance > dangerRadiusSqr) continue;

                float awayDistance = Mathf.Sqrt(sqrAwayDistance);
                float currentPanic = 1f - (awayDistance / _dangerRadius);
                
                if (currentPanic > maxPanic) 
                    maxPanic = currentPanic;

                float distanceWeight = 1f / (sqrAwayDistance + _epsilon);

                float sizeRatio = (threat.Size.Current - selfSize) / (float)Mathf.Max(selfSize, 1);
                float sizeBoost = Mathf.Lerp(_minBoost, _maxThreatRepulsionBoost, Mathf.Clamp01(sizeRatio));

                sum += away.normalized * distanceWeight * sizeBoost;
            }

            panic = maxPanic;
            return sum;
        }
    }
}