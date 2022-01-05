using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dico.HyperCasualGame.Pool;

namespace Dico.Helper
{
	public class StopFalling : SpawnableObject
	{
        Rigidbody rgBody;

        void Start()
		{
            rgBody = GetComponentInChildren<Rigidbody>();

        }

		void FixedUpdate()
        {
            var currentVelocity = rgBody.velocity;

            if (currentVelocity.y <= 0f)
                return;

            currentVelocity.y = 0f;

            rgBody.velocity = currentVelocity;
        }
    }
}
