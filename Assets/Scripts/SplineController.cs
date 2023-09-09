using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SplineController : MonoBehaviour
{
    [SerializeField] private SplineData startSpline;
    [SerializeField] private SplineData finishSpline;
    [SerializeField] private List<SplineData> Splines = new();
    [SerializeField] private float posForReset;
    private int maxCountSplines = 0;
    private int countSplines = 0;
    private bool lastSplineIsUpOrDown;
    private SplineData oldestSpline;
    private SplineData oldSpline;
    private SplineData lastSpline;
    private bool stopGame;
    private bool loopGame;
    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        startSpline.splineInstantiate.Clear();
        Invoke(nameof(StartSplineRandomize),0.05f);
        Observer.StartGame += StartGame;
        Observer.ÑallNewPath += NewPath;
        Observer.EndGame += (x) => stopGame = true;
        Observer.SetCountSplines += (x) =>
        {
            loopGame = x == 0;
            maxCountSplines = x;
            Debug.Log($"maxCountSplines {x} loopGame {loopGame}");
        };
    }
    private void StartSplineRandomize()
    {
        startSpline.splineInstantiate.Randomize();
    }
    public void StartGame()
    {
        oldestSpline = startSpline;
        NewPath();
    }
    private void NewPath()
    {
        if (countSplines > maxCountSplines || stopGame)
            return;

        lastSpline = GetFreeSplineData();
        if (!loopGame)
        {
            countSplines++;
            Debug.Log($"now countSplines {countSplines}");
        }

        if (oldSpline == null)
        {
            lastSpline.transform.position = oldestSpline.transform.position + new Vector3(0, oldestSpline.SplineHight, oldestSpline.SplineLength);
            oldSpline = lastSpline;
            NewPath();
            return;
        }
        else
        {
            lastSpline.transform.position = oldSpline.transform.position + new Vector3(0, oldSpline.SplineHight, oldSpline.SplineLength);
        }

        if (oldestSpline.transform.position.y > posForReset || oldestSpline.transform.position.y < -posForReset)
            ResetPositionYZ(oldestSpline.transform.position.y, 0);
        if (oldestSpline.transform.position.z > posForReset || oldestSpline.transform.position.z < -posForReset)
            ResetPositionYZ(0, oldestSpline.transform.position.z);

        var Matrix1 = oldestSpline.transform.localToWorldMatrix;
        var Matrix2 = oldSpline.transform.localToWorldMatrix;
        var Matrix3 = lastSpline.transform.localToWorldMatrix;
        // Create a SplinePath from a subset of Splines
        SplinePath path = new SplinePath(new[]
        {
                new SplineSlice<Spline>(oldestSpline.splineContainer.Splines[0], new SplineRange(0, oldestSpline.splineContainer.Splines[0].Count), Matrix1),
                new SplineSlice<Spline>(oldSpline.splineContainer.Splines[0], new SplineRange(0, oldSpline.splineContainer.Splines[0].Count), Matrix2),
                new SplineSlice<Spline>(lastSpline.splineContainer.Splines[0], new SplineRange(0, lastSpline.splineContainer.Splines[0].Count), Matrix3),
            });
        Observer.SetNewPath(path);
        //Debug.Log("path " + path.Count);
        oldestSpline = oldSpline;
        oldSpline = lastSpline;
    }

    private SplineData GetFreeSplineData()
    {
        SplineData FreeSpline;
        SplineData[] freeSplines;
        if (countSplines < maxCountSplines || loopGame)
        {
            if (lastSplineIsUpOrDown)
                freeSplines = Splines.Where(sp => sp.isFree && !sp.isUpOrDown).ToArray();
            else
            {
                freeSplines = Splines.Where(sp => sp.isFree).ToArray();
            }

            if (freeSplines.Length <= 0)
                return null;
            int i = Random.Range(0, freeSplines.Length);
            FreeSpline = freeSplines[i];
            lastSplineIsUpOrDown = FreeSpline.isUpOrDown;
        }
        else
        {
            FreeSpline = finishSpline;
        }
        FreeSpline.isFree = false;
        FreeSpline.splineInstantiate.Randomize();
        FreeSpline.gameObject.SetActive(true);
        return FreeSpline;
    }
    private void ResetPositionYZ(float y, float z)
    {
        Observer.ResetPositionAllYZ(y,z);
        Debug.Log("ResetPositionYZ" + new Vector3(0, y, z));
        foreach (var item in Splines)
        {
            if(item.isActiveAndEnabled)
                item.transform.position -= new Vector3(0, y, z);
        }
    }
    private void OnDestroy()
    {
        Observer.EndGame -= (x) => stopGame = true;
        Observer.SetCountSplines -= (x) =>
        {
            loopGame = x == 0;
            maxCountSplines = x;
        };
    }
}
