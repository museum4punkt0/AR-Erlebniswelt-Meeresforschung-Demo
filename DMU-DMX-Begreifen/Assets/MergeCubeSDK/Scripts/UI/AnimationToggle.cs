using UnityEngine;

public class AnimationToggle : MonoBehaviour
{
	public Animator animator;
	public string animationOneName;
	public string animationTwoName;

	private bool isDefaultState = true;

	public void ToggleAnimation()
	{
		if ( isDefaultState )
		{
			animator.Play( animationOneName );
		}
		else
		{
			animator.Play( animationTwoName );
		}

		isDefaultState = !isDefaultState;
	}

	public void PlayDefaultStateAnimation(bool useDefaultState)
	{
		isDefaultState = useDefaultState;
		ToggleAnimation();
	}
}
