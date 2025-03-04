using DG.Tweening;
using ModestTree;
using UnityEngine;

public class ViewSlider2D : View
{
	[field: SerializeField]
	public GameObject FillArea { get; private set; }
	[field: SerializeField]
	public float MaxValue { get; private set; } = -1f;
	[field: SerializeField]
	public float MinValue { get; private set; } = -1f;
	[field: SerializeField]
	public float CurrentValue { get; private set; } = -1f;
	[field: SerializeField]
	public float AnimationTime { get; private set; } = AnimationUtils.ANIM_SLIDER_CHANGE;

	private Vector3 _startScale;
	private float _currentScaleX;
	private float _newScaleX;
	private Sequence _sequence;

	private float _range => MaxValue - MinValue;

	public void Awake()
	{
		Assert.IsNotNull(FillArea);

		_startScale = FillArea.transform.localScale;
	}

	public void OnDestroy()
	{
		DOTween.Kill(this);
	}

	public void SetStartValue(float currentValue, float maxValue, float minValue = 0)
	{
		MaxValue = maxValue;
		MinValue = minValue;
		SetCurrentValue(currentValue);
	}

	public void SetCurrentValue(float currentValue, bool isFast = true)
	{
		var normalizedValue = (currentValue - MinValue) / _range;
		_newScaleX = _startScale.x * normalizedValue;
		CurrentValue = currentValue;

		if (isFast)
		{
			SetScaleX(_newScaleX);
			UpdateCurrentScaleX();
			return;
		}
		DOTween.Kill(this);
		DOTween.Sequence()
			.Append(DOTween.To(SetScaleX, _currentScaleX, _newScaleX, AnimationTime))
			.OnComplete(UpdateCurrentScaleX)
			.SetId(this);
	}

	private void UpdateCurrentScaleX()
	{
		_currentScaleX = _newScaleX;
	}

	private void SetScaleX(float scaleX)
	{
		var currentScale = new Vector3(scaleX, _startScale.y, _startScale.z);
		FillArea.transform.localScale = currentScale;
	}
}