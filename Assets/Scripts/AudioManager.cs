using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static bool IsMute;
    public static AudioManager inst;
    [SerializeField] private Button audioOn;
    [SerializeField] private Button audioOff;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourcePlay1Shot;

    [SerializeField] private AudioClip[] audioClipGetCoin;
    [SerializeField] private AudioClip audioClipLoseGame;
    [SerializeField] private AudioClip audioClipWinGame;
    [SerializeField] private AudioClip audioClipMenu;
    private void Start()
    {
        inst = this;
        IsMute = PlayerPrefs.GetInt("IsMute", 0) == 1;
        audioSourceMusic.mute = IsMute;
        audioSourcePlay1Shot.mute = IsMute;
        audioOn.gameObject.SetActive(!IsMute);
        audioOff.gameObject.SetActive(IsMute);
        audioOn.onClick.AddListener(() =>
        {
            audioOff.gameObject.SetActive(true);
            audioOn.gameObject.SetActive(false);
            audioSourceMusic.mute = true;
            audioSourcePlay1Shot.mute = true;
            Play1ShotMenu();
        });

        audioOff.onClick.AddListener(() =>
        {
            audioOff.gameObject.SetActive(false);
            audioOn.gameObject.SetActive(true);
            audioSourceMusic.mute = false;
            audioSourcePlay1Shot.mute = false;
            Play1ShotMenu();
        });
    }
    public void Play1ShotGetCoin()
    {
        audioSourcePlay1Shot.PlayOneShot(audioClipGetCoin[Random.Range(0, audioClipGetCoin.Length)]);
    }
    public void Play1ShotLoseGame()
    {
        audioSourcePlay1Shot.PlayOneShot(audioClipLoseGame);
    }
    public void Play1ShotWinGame()
    {
        audioSourcePlay1Shot.PlayOneShot(audioClipWinGame);
        audioSourceMusic.Stop();
    }
    public void Play1ShotMenu()
    {
        audioSourcePlay1Shot.PlayOneShot(audioClipMenu);
    }
}
