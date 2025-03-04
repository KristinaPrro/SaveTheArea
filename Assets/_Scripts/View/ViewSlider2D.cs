using DG.Tweening;
using ModestTree;
using UnityEngine;

public class ViewSlider2D : View
{
	[field: SerializeField]
	public GameObject FillArea { get; private set; }
	[field: SerializeField]
	public float AnimationTime { get; private set; } = Utils.FLOAT_DEFAULT_VALUE;

	public Vector3 StartScale { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(FillArea);

		StartScale = FillArea.transform.localScale;
	}
}