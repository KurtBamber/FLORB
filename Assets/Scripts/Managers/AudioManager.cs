using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip pickupClip;

    public void PlayPickupSound(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(pickupClip, pos, 1f);
    }
}
