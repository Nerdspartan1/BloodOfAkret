using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerkRarity
{
	Common,
	Uncommon,
	Rare,
	Legendary
}

[CreateAssetMenu(fileName ="Perk",menuName = "Perk")]
public class Perk : ScriptableObject
{
	public string Name;
	public PerkRarity Rarity;
	public Sprite Icon;
	[TextArea(4,6)]
	public string Description;
}
