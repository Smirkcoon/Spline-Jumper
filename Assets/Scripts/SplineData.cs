using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineData : MonoBehaviour
{
    public float SplineLength;
    public float SplineHight;
    public SplineContainer splineContainer;
    public SplineInstantiate splineInstantiate;
    public bool isFree = true;
    public bool isUpOrDown;
    public bool isFinish = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("OnTriggerExit " + gameObject.name);
            if (isFinish)
            {
                Observer.EndGame(true);
                return;
            }
            Observer.ÑallNewPath();
            StartCoroutine(SetFree());
            IEnumerator SetFree()
            {
                yield return new WaitForSeconds(3);
                isFree = true;
                gameObject.SetActive(false);
            }
        }
    }
}
