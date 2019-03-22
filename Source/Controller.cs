/*
 * User: Phomor
 * Date: 22.06.2018
 * Time: 18:18
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using Harmony;
using UnityEngine;
using Verse;
using RimWorld;
using static CraftingQualityRebalanced.Settings;

namespace CraftingQualityRebalanced
{
	/// <summary>
	/// Crafting and Building Quality is less random, higher skill Pawns can't make awful things anymore.
	/// In Detail: Pawns with Skill Level 17+ will make at least good things, Skill Level 13+ will make at least normal things, 
	/// Skill Level 9+ will make at least poor things, Skill Level 7+ will make at least shoddy things
	/// </summary>
	public class Controller : Mod
	{
		public Settings settings;
		
		public Controller(ModContentPack content) : base (content)
		{
			var harmony = HarmonyInstance.Create("rimworld.phomor.craftingqualityrebalanced");
			var redoQuality = typeof(QualityUtility).GetMethod("GenerateQualityCreatedByPawn", new Type[] { typeof(int), typeof(bool) });
			var postfix = typeof(HarmonyPatches).GetMethod("Postfix");
			harmony.Patch(redoQuality, null, new HarmonyMethod(postfix));
			var supressMessages = typeof(QualityUtility).GetMethod("SendCraftNotification");
			var prefix = typeof(HarmonyPatches).GetMethod("Prefix");
			harmony.Patch(supressMessages, new HarmonyMethod(prefix), null);
			settings = GetSettings<Settings>();
			updatePatches();
		}
		
		public override string SettingsCategory()
    	{
			return "CraftingQualityRebalanced".Translate();
    	}
		
		public override void DoSettingsWindowContents(Rect inRect)
		{
			settings.DoWindowContents(inRect);
			updatePatches();
		}
		
		public void updatePatches()
		{
			HarmonyPatches.minSkillLegendary = (int)settings.GetSkillValue(SkillLevel.Legendary);
			HarmonyPatches.minSkillMasterwork = (int)settings.GetSkillValue(SkillLevel.Masterwork);
            HarmonyPatches.minSkillExcellent = (int)settings.GetSkillValue(SkillLevel.Excellent);
            HarmonyPatches.minSkillGood = (int)settings.GetSkillValue(SkillLevel.Good);
            HarmonyPatches.minSkillNormal = (int)settings.GetSkillValue(SkillLevel.Normal);
            HarmonyPatches.minSkillPoor = (int)settings.GetSkillValue(SkillLevel.Poor);
            HarmonyPatches.legendaryChanceAt20 = (float)(settings.legendaryChance / 100f);
			if(HarmonyPatches.minSkillLegendary != 21)
				HarmonyPatches.gradientLegendary = (float)(HarmonyPatches.legendaryChanceAt20/(20 - (HarmonyPatches.minSkillLegendary - 1)));
			HarmonyPatches.supressMasterwork = settings.supressMasterworkMessages;
			HarmonyPatches.supressLegendary = settings.supressLegendaryMessages;
		}
	}
}
