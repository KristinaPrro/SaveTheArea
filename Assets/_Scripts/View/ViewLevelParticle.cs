using ModestTree;
using UnityEngine;

public class ViewLevelParticle : View
{
	[field: SerializeField]
	public ParticleSystem ParticleSystemWin { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(ParticleSystemWin);
	}
}