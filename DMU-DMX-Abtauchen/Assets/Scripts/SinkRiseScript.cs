using UnityEngine;

/**
 * Script to play the animation on the last simulation scene
 */
public class SinkRiseScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private static readonly int SinkRise = Animator.StringToHash("SinkRise");

    public void Play()
    {
        anim.SetTrigger(SinkRise);
    }

}
