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
        var selectedMenuMusic = PlayerData.LostGame ? gameOverMusic : menuMusic;
        
        if (_currentMusic)
        {
            SoundUtility.Instance.Crossfade(_currentMusic, selectedMenuMusic, crossfade);
            _currentMusic = selectedMenuMusic;
        }
        else
        {
            _currentMusic = selectedMenuMusic;
            selectedMenuMusic.Play();
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