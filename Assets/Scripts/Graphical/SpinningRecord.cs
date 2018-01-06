using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningRecord : MonoBehaviour {

    public float labelRotationSpeed = 5f;
    public float baseRotationSpeed = 10f;

    private Transform recordBase;
    private Transform recordLabel;

    private bool lastRotatedUp = false;
    private Quaternion origRotation;

    public bool isSpinning;

    void Awake() {
        foreach (Transform child in transform) {
            if (child.CompareTag("Record Base")) {
                recordBase = child;
            } else if (child.CompareTag("Record Label")) {
                recordLabel = child;
            }
            Debug.Log(child.tag);
        }
        origRotation = recordBase.rotation;
    }

    public void SetSpinning(bool isSpinning) {
        this.isSpinning = isSpinning;
    }

	// Update is called once per frame
	void Update () {
        if (isSpinning) {
            recordLabel.Rotate(Vector3.forward * labelRotationSpeed * Time.deltaTime);
            if (lastRotatedUp)
                recordBase.Rotate(-Vector3.forward * baseRotationSpeed * Time.deltaTime);
            else
                recordBase.Rotate(Vector3.forward * baseRotationSpeed * Time.deltaTime);
            lastRotatedUp = !lastRotatedUp;
        }
    }
}
