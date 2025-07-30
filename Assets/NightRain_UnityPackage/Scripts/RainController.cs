
using UnityEngine;

public class RainController : MonoBehaviour
{
    public ParticleSystem rainParticles;
    public AudioSource rainSound;

    public void StartRain()
    {
        rainParticles.Play();
        rainSound.Play();
    }

    public void StopRain()
    {
        rainParticles.Stop();
        rainSound.Stop();
    }
}
