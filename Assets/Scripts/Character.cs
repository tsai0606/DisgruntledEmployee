﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;

public class Character: MonoBehaviour {
	public GameObject AreaOfEffectPrefab;
	public float moveSpeed = 2f;
	public int nextPoint = 0;
	public float infectRadius = 0;

	private Vector3[] path;
    private IsoTransform _isoTransform;
	private GameObject aoeGO = null;
	private AreaOfEffect aoeScript;

    void Awake() {
        _isoTransform = this.GetOrAddComponent<IsoTransform>(); //avoids polling the IsoTransform component per frame
    }

	void Start() {
		path = GetComponentInParent<CharacterPath>().path;
		//nextPoint = Mathf.RoundToInt(Random.Range(0f, path.Length - 1));
		_isoTransform.Position = path[nextPoint];

		if (infectRadius > 0f) {
			this.Infect(infectRadius);
		}
	}

	void Update () {
        _isoTransform.Position = Vector3.MoveTowards(_isoTransform.Position, path[nextPoint], moveSpeed * Time.deltaTime);

        if (_isoTransform.Position == path[nextPoint]) {
			nextPoint = (nextPoint + 1) % path.Length;
		}
	}

	public void Infect(float radius) {
		infectRadius = radius;

		aoeGO = GameObject.Instantiate(AreaOfEffectPrefab);
		aoeScript = aoeGO.GetComponent<AreaOfEffect>();
		aoeScript.UpdateScale(radius);
		aoeGO.transform.parent = this.transform;
		aoeGO.transform.localPosition = new Vector3(0, -0.5f, 0);
	}

	void OnDestroy() {
		if (aoeGO != null) {
			Destroy(aoeGO);
			aoeGO = null;
		}
	}
}