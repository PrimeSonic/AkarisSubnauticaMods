﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CyclopsCameraDroneMod
{
    public class TractorBeamedObject : MonoBehaviour
    {
        public Rigidbody rb;
        public Transform targetTransform;

        private float _timeLastRefreshed;
        private const float _maxRefreshTime = 0.5f;

        private void Start()
        {
            rb.isKinematic = false;
        }

        public void Refresh() // stop this component from destroying itself
        {
            _timeLastRefreshed = Time.time;
        }

        private void FixedUpdate()
        {
            if (Time.time > _timeLastRefreshed + _maxRefreshTime || targetTransform == null)
            {
                Destroy(this);
            }
            rb.AddForce(CalculateForce(), ForceMode.Force);
            DampenVelocity();
        }

        private Vector3 CalculateForce()
        {
            var normalized = (targetTransform.position - transform.position).normalized;
            var strength = Mathf.Clamp(TractorBeam.force * rb.mass, TractorBeam.force, TractorBeam.maxForce);
            return normalized * strength;
        }

        private void DampenVelocity()
        {
            var relativeToCamera = targetTransform.InverseTransformVector(rb.velocity);
            var dampenened = new Vector3(relativeToCamera.x * (1 - Time.fixedDeltaTime * TractorBeam.dampeningStrength), relativeToCamera.y * (1 - Time.fixedDeltaTime * TractorBeam.dampeningStrength), relativeToCamera.z);
            rb.velocity = targetTransform.TransformVector(dampenened);
        }
    }
}
