using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class PreySteering
    {
        private float _absorbRadius;
        private float _minBoost;
        private float _maxPreyAttractionBoost;
        private float _mainTargetHuntBias;
        private float _epsilon;
        
        internal PreySteering(float absorbRadius, float minBoost, 
            float maxPreyAttractionBoost,  float mainTargetHuntBias, float epsilon)
        {
            _absorbRadius = absorbRadius;
            _minBoost = minBoost;
            _maxPreyAttractionBoost = maxPreyAttractionBoost;
            _mainTargetHuntBias = mainTargetHuntBias;
            _epsilon = epsilon;
        }
        
        internal Vector2 ComputePreyAttraction(IReadOnlyCollection<IAbsorber> absorbers, IAbsorbable mainTarget, 
            Vector2 selfPosition, int selfSize)
        {
            float preyRadiusSqr = _absorbRadius * _absorbRadius;
            Vector2 sum = Vector2.zero;

            foreach (IAbsorber prey in absorbers)
            {
                if (prey == null || !prey.Owner.IsActive) continue;
                if (prey.Owner.CurrentSize >= selfSize) continue;

                Vector2 toPrey = prey.Owner.CurrentPosition - selfPosition;
                float sqrPreyDistance = toPrey.sqrMagnitude;
                if (sqrPreyDistance < DirectionMath.MinSqrMagnitudeForDirection || sqrPreyDistance > preyRadiusSqr) continue;

                float distanceWeight = 1f / (sqrPreyDistance + _epsilon);

                float sizeRatio = (selfSize - prey.Owner.CurrentSize) / (float)Mathf.Max(selfSize, 1);
                float weightBoost = Mathf.Lerp(_minBoost, _maxPreyAttractionBoost, Mathf.Clamp01(sizeRatio));
                
                sum += toPrey.normalized * distanceWeight * weightBoost;
            }
            
            if (mainTarget != null && mainTarget.Owner.IsActive && selfSize > mainTarget.Owner.CurrentSize)
            {
                Vector2 toMainTarget = mainTarget.Owner.CurrentPosition - selfPosition;
                
                if (toMainTarget.sqrMagnitude > DirectionMath.MinSqrMagnitudeForDirection)
                    sum += toMainTarget.normalized * _mainTargetHuntBias;
            }

            return sum;
        }
    }
}