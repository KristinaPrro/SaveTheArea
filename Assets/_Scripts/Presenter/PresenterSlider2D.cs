using DG.Tweening;
using System;
using UnityEngine;

public class PresenterSlider2D : PresenterBase<ViewSlider2D>
{
	private Sequence _sequence;
	private float _maxValue = Utils.FLOAT_DEFAULT_VALUE;
	private float _minValue = Utils.FLOAT_DEFAULT_VALUE;
	private float _currentScaleX;
	private float _newScaleX;
	private float _range;

	private float _animationTime => View.AnimationTime;
	private Vector3 _startScale => View.StartScale;

	public float CurrentValue { get; private set; } = Utils.FLOAT_DEFAULT_VALUE;

	public PresenterSlider2D(ViewSlider2D view) : base(view)
	{
	}

	public override void Dispose()
	{
		DOTween.Kill(this);
		base.Dispose();
	}

	public void SetVisible(bool isVisible)
	{
		View.gameObject.SetActive(isVisible);
	}

	public void SetStartValue(float value, float maxValue, float minValue = 0)
	{
		_maxValue = maxValue;
		_minValue = minValue;
		_range = maxValue - minValue;

		SetCurrentValue(value);
	}

	public void SetCurrentValue(float value, bool isFast = true)
	{
		if (_minValue == _maxValue)
		{
			this.LogError($"Set correct start value! _minValue:{_minValue}; _maxValue:{_maxValue} ({value})");
			return;
		}
		
		var newvalue = Math.Clamp(value, _minValue, _maxValue);

		if(CurrentValue == newvalue)
		{
			this.LogWarning($"{newvalue}({value}) value has already been set! \n" +
				$"_currentScaleX:{_currentScaleX}; _newScaleX:{_newScaleX}; StartScale:{View.StartScale};");
			return;
		}

		var normalizedValue = (newvalue - _minValue) / _range;
		var newScaleX = _startScale.x * normalizedValue;
		_newScaleX = newScaleX;
		CurrentValue = newvalue;

		if (isFast || _animationTime == Utils.FLOAT_DEFAULT_VALUE)
		{
			SetScaleX(newScaleX);
			UpdateCurrentScaleX();
			return;
		}

		DOTween.Kill(this);
		DOTween.Sequence()
			.Append(DOTween.To(SetScaleX, _currentScaleX, newScaleX, _animationTime))
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

		View.FillArea.transform.localScale = currentScale;
	}
}