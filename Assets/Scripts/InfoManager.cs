using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class InfoManager : MonoBehaviour
{
    [SerializeField] private GameObject info;
    [SerializeField] private TextMeshPro prefabAddScore;

    private void Start()
    {
        Observer.StartGame += StartGame;
        Observer.AddScore += NewInfoAddScore;
    }
    public void StartGame()
    {
        info.SetActive(true);
        Invoke(nameof(SetInfoOff), 2);
    }

    private void SetInfoOff()
    {
        info.SetActive(false);
    }

    public void NewInfoAddScore(int score, Vector3 playerPos)
    {
        //Create a new random color
        byte r = (byte)Random.Range(0, 256);
        byte g = (byte)Random.Range(0, 256);
        byte b = (byte)Random.Range(0, 256);
        byte a = 0;
        Color randomColor = new Color32(r, g, b, a);
        TextMeshPro text = Instantiate(prefabAddScore, playerPos + new Vector3(0,1f,0),Quaternion.identity);
        text.text = score.ToString();
        text.color = randomColor;
        text.DOFade(255, 0.4f);
        text.gameObject.transform.DOMoveX(text.gameObject.transform.position.x - 0.5f, 0.5f);
        text.gameObject.transform.DOMoveZ(text.gameObject.transform.position.z + 1, 0.5f)
            .OnComplete(() => Destroy(text.gameObject));
    }
}
