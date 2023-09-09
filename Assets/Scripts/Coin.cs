using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public enum CoinType
    {
        Bronze,
        Silver,
        Gold
    }

    [SerializeField] private CoinType coinType;
    private float rotationSpeed = 30f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Observer.AddScore((int)coinType +1, other.transform.position);
            gameObject.SetActive(false);
        }
    }
}
