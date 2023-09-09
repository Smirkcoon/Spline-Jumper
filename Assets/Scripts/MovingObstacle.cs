using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float moveSpeed = 3f; // Movement speed
    public float moveDistance = 5f; // Distance the obstacle moves left and right

    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.right * moveDistance;

        StartCoroutine(MoveObstacle());
    }

    private IEnumerator MoveObstacle()
    {
        while (true)
        {
            // Move to the right
            while (transform.position.x < endPos.x)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            // Move to the left
            while (transform.position.x > startPos.x)
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
