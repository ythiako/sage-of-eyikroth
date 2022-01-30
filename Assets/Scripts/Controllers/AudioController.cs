using UnityEngine;
using Utils;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private AudioSource gameOverMusic;

    private AudioSource _currentMusic;
    
    public void PlayMenuMusic(float crossfade = 0.5f)
    {
        if (_currentMusic)
        {
            SoundUtility.Instance.Crossfade(_currentMusic, menuMusic, crossfade);
            _currentMusic = menuMusic;
        }
        else
        {
            _currentMusic = menuMusic;
            menuMusic.Play();
        }
    }

    public void PlayGameMusic(float crossfade = 0.5f)
    {
        if (_currentMusic)
        {
            SoundUtility.Instance.Crossfade(_currentMusic, gameMusic, crossfade);
            _currentMusic = gameMusic;
        }
        else
        {
            _currentMusic = gameMusic;
            gameMusic.Play();
        }
    }

    public void PlayGameOverMusic(float crossfade = 0.5f)
    {
        if (_currentMusic)
        {
            SoundUtility.Instance.Crossfade(_currentMusic, gameOverMusic, crossfade);
            _currentMusic = gameOverMusic;
        }
        else
        {
            _currentMusic = gameOverMusic;
            gameOverMusic.Play();
        }
    }
}