using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour {

    private int currentlySelected = -1;
    private int previouslySelected = -1;

    public float timeScale;
    public float selectedScaleMultiplier;
    public Vector3 unselectedScale;
    public List<GameObject> records;

    private float spinTimer = 0.5f; //HACK to get records to same spin
    private bool spinTimerDone;

    // Use this for initialization
    void Awake () {
        records[0].GetComponent<SpinningRecord>().SetSpinning(true);
        records[1].GetComponent<SpinningRecord>().SetSpinning(true);
        records[2].GetComponent<SpinningRecord>().SetSpinning(true);
    }

	
    public void setSelected(int selected) {
        if (selected != currentlySelected) {
            if (currentlySelected >= 0) {
                StartCoroutine(LerpDown(records[currentlySelected]));
                records[currentlySelected].GetComponent<SpinningRecord>().SetSpinning(false);
            }
            previouslySelected = currentlySelected;
            currentlySelected = selected;
            StartCoroutine(LerpUp(records[currentlySelected]));
            records[currentlySelected].GetComponent<SpinningRecord>().SetSpinning(true);
        }
    }

    void Update() {
        spinTimer -= Time.deltaTime;
        if (spinTimer < 0 && !spinTimerDone) {
            records[0].GetComponent<SpinningRecord>().SetSpinning(false);
            records[1].GetComponent<SpinningRecord>().SetSpinning(false);
            records[2].GetComponent<SpinningRecord>().SetSpinning(false);
            records[currentlySelected].GetComponent<SpinningRecord>().SetSpinning(true);
        }
    }

    IEnumerator LerpUp(GameObject obj) {
        float progress = 0;

        while (progress <= 1) {
            obj.transform.localScale = Vector3.Lerp(unselectedScale, unselectedScale * selectedScaleMultiplier, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
        //transform.localScale = unselectedScale * selectedScaleMultiplier;
    }

    IEnumerator LerpDown(GameObject obj) {
        float progress = 0;

        while (progress <= 1) {
            obj.transform.localScale = Vector3.Lerp(unselectedScale * selectedScaleMultiplier, unselectedScale, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
        //transform.localScale = unselectedScale;
    }

}
