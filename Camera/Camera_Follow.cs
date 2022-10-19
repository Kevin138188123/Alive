using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] float followLerpFactor = 5f;

	Transform thisTransform;


	void Start()
	{
		if (!target) enabled = false;

		thisTransform = transform;
	}

	void FixedUpdate()
	{
		thisTransform.position = Vector3.Lerp(thisTransform.position, target.position, Time.deltaTime * followLerpFactor);
	}
}

