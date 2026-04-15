using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class AbsorbableAttractionSteering
    {
        private float _absorbRadius;
        private float _minAttractionBoost;
        private float _maxAttractionBoost;
        private float _mainTargetAttractionBias;
        private float _epsilon;
        
        internal AbsorbableAttractionSteering(float absorbRadius, float minAttractionBoost, 
            float maxAttractionBoost,  float mainTargetAttractionBias, float epsilon)
        {
            _absorbRadius = absorbRadius;
            _minAttractionBoost = minAttractionBoost;
            _maxAttractionBoost = maxAttractionBoost;
            _mainTargetAttractionBias = mainTargetAttractionBias;
            _epsilon = epsilon;
        }
        
        internal Vector2 Compute(IReadOnlyCollection<IAbsorbable> absorbables, IAbsorbable mainTarget, IAbsorber self)
        {
            float absorbableRadiusSqr = _absorbRadius * _absorbRadius;
            Vector2 sum = Vector2.zero;

            foreach (IAbsorbable absorbable in absorbables)
            {
                if (absorbable == null || !self.CanAbsorb(absorbable) || !absorbable.Body.IsActive) continue;

                Vector2 toAbsorbable = absorbable.Body.CurrentPosition - self.Body.CurrentPosition;
                float sqrAbsorbableDistance = toAbsorbable.sqrMagnitude;
                
                if (sqrAbsorbableDistance < DirectionMath.MinSqrMagnitudeForDirection || sqrAbsorbableDistance > absorbableRadiusSqr) continue;

                float distanceWeight = 1f / (sqrAbsorbableDistance + _epsilon);
                float sizeRatio = (self.Body.Size.Current - absorbable.Body.Size.Current) / (float)Mathf.Max(self.Body.Size.Current, 1);
                float weightBoost = Mathf.Lerp(_minAttractionBoost, _maxAttractionBoost, Mathf.Clamp01(sizeRatio));
                
                sum += toAbsorbable.normalized * distanceWeight * weightBoost;
            }
            
            if (mainTarget != null && mainTarget.Body.IsActive && self.CanAbsorb(mainTarget))
            {
                Vector2 toMainTarget = mainTarget.Body.CurrentPosition - self.Body.CurrentPosition;
                   
                if (toMainTarget.sqrMagnitude > DirectionMath.MinSqrMagnitudeForDirection)
                    sum += toMainTarget.normalized * _mainTargetAttractionBias;
            }

            return sum;
        }
    }
}