using System.Collections;
using UnityEngine;
using MergeCube;

public class MergeReticle : MonoBehaviour
{
	public static MergeReticle instance;
	public Transform reticle;
	public Sprite fullScreenSprite;
	public Sprite vrScreenSprite;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		defaultScale = reticle.localScale;
		MergeCubeSDK.instance.OnViewModeSwap += ViewModeSwitch;
	}

	private void OnDestroy()
	{
		MergeCubeSDK.instance.OnViewModeSwap -= ViewModeSwitch;
	}


	private bool isEnabledInPhoneMode = true;
	private bool isVRMode;

	private void ViewModeSwitch(bool isVRModeTp)
	{
		isVRMode = isVRModeTp;
		reticle.GetComponent<SpriteRenderer>().sprite = isVRMode ? vrScreenSprite : fullScreenSprite;
		SetBackState();
	}

	private bool gameLock;

	public void GameOverwrite(bool lockTp, bool showReticleTp = false)
	{
		gameLock = lockTp;
		if (lockTp)
		{
			SetReticleActive(showReticleTp, true);
		}
		else
		{
			SetBackState();
		}
	}

	private void SetBackState()
	{
		if (isVRMode)
		{
			SetReticleActive(true);
		}
		else
		{
			SetReticleActive(isEnabledInPhoneMode);
		}
	}

	public void EnableReticle(bool isEnableTp)
	{
		isEnabledInPhoneMode = isEnableTp;
		if (!isVRMode)
		{			
			SetReticleActive(isEnabledInPhoneMode);
		}
	}

	private void SetReticleActive(bool isActive, bool isGameLockSet = false)
	{
		if (gameLock && !isGameLockSet)
		{
			return;
		}
		reticle.gameObject.SetActive(isActive);
	}
	//Animations
	public void OnHoverAction()
	{
		StartScaleLerp(maxScaleMult, scaleUpDuration);
	}

	public void OffHoverAction()
	{
		StartScaleLerp(minScaleMult, scaleDownDuration);
	}

	public void OnClickAction()
	{
		reticle.transform.localScale = defaultScale * .5f;
	}

	public void OffClickAction()
	{
		reticle.transform.localScale = defaultScale;
	}


	private Vector3 defaultScale;
	public float maxScaleMult = 1.5f;
	public float minScaleMult = .8f;

	[Space(20)]
	public float scaleUpDuration = 1f;
	public float scaleDownDuration = 1f;


	private IEnumerator ScaleLerp(float targetScaleMult, float timerDuration)
	{
		Vector3 startingScale = reticle.localScale;
		Vector3 targetScale = defaultScale * targetScaleMult;
		float time = 0f;

		while ((time / timerDuration) < 1f)
		{
			reticle.localScale = Vector3.Lerp(startingScale, targetScale, time / timerDuration);
			time += Time.deltaTime;
			yield return null;
		}
		reticle.localScale = targetScale;
	}


	private Coroutine scaleLerpCo;

	private void StartScaleLerp(float targetScaleMult, float timerDuration)
	{
		StopScaleLerp();
		scaleLerpCo = StartCoroutine(ScaleLerp(targetScaleMult, timerDuration));
	}


	private void StopScaleLerp()
	{
		if (scaleLerpCo != null)
		{
			StopCoroutine(scaleLerpCo);
		}
	}
		
}
