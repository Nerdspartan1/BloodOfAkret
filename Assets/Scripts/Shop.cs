using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject PerkCardPrefab;

	[Header("References")]
	public Transform ArrayPanel;

	public List<Perk> UnlockedPerks;
	public int MaxPerksForSale;

	[Range(0, 1)]
	public float CommonProbability = 1;
	[Range(0, 1)]
	public float UncommonProbability;
	[Range(0, 1)]
	public float RareProbability;
	[Range(0, 1)]
	public float LegendaryProbability;

	[Range(0, 1)]
	public float CommonProbability_BossLoot = 1;
	[Range(0, 1)]
	public float UncommonProbability_BossLoot;
	[Range(0, 1)]
	public float RareProbability_BossLoot;
	[Range(0, 1)]
	public float LegendaryProbability_BossLoot;

	public GameObject Heal;

	public void PresentPerks(bool bossLoot)
	{
		if (WaveManager.Instance.Wave >= 16) Heal.SetActive(false);

		List<Perk> randomPerks = new List<Perk>(UnlockedPerks);
		randomPerks.RandomizeList();

		foreach(Transform card in ArrayPanel)
		{
			Destroy(card.gameObject);
		}

		int nbPerksForSale = Mathf.Min(MaxPerksForSale, randomPerks.Count);

		int perksChosen = 0;

		int i = 0;
		while(perksChosen < nbPerksForSale)
		{
			Perk perk = randomPerks[i % randomPerks.Count];
			float p=1;
			if (!bossLoot)
			{
				switch (perk.Rarity)
				{
					case PerkRarity.Common: p = CommonProbability; break;
					case PerkRarity.Uncommon: p = UncommonProbability; break;
					case PerkRarity.Rare: p = RareProbability; break;
					case PerkRarity.Legendary: p = LegendaryProbability; break;
				}
			}
			else
			{
				switch (perk.Rarity)
				{
					case PerkRarity.Common: p = CommonProbability_BossLoot; break;
					case PerkRarity.Uncommon: p = UncommonProbability_BossLoot; break;
					case PerkRarity.Rare: p = RareProbability_BossLoot; break;
					case PerkRarity.Legendary: p = LegendaryProbability_BossLoot; break;
				}
			}

			if(Random.value < p)
			{
				PerkCard card = Instantiate(PerkCardPrefab, ArrayPanel).GetComponent<PerkCard>();
				card.SetPerk(perk);
				perksChosen++;
				randomPerks.Remove(perk);
			}

			if (++i > 100000) break;
		}

	}
}
