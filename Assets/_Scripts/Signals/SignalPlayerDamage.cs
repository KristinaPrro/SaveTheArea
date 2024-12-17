public class SignalPlayerDamage
{
	public int DamageElementId { get; set; }
	public int TargetId { get; set; }
	
	public SignalPlayerDamage(int damageElementId, int targetId)
	{
		DamageElementId = damageElementId;
		TargetId = targetId;
	}
}