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

namespace CraftingQualityRebalanced
{
	public class Settings : Verse.ModSettings
    {
        public enum SkillLevel
        {
            Poor = 0,
            Normal,
            Good,
            Excellent,
            Masterwork,
            Legendary,
        }

        private float[] minSkills = new float[] { 9, 13, 17, 21, 22, 19 };
        public float legendaryChance = 5;
		public bool supressMasterworkMessages = false;
		public bool supressLegendaryMessages = false;

        public float GetSkillValue(SkillLevel level) => this.minSkills[(int)level];
        public void SetSkillValue(SkillLevel level, float value)
        {
            this.minSkills[(int)level] = value;
            for (int i = (int)level - 1; i >= 0; --i)
            {
                if (minSkills[i] > value)
                    minSkills[i] = value;
                else
                    break;
            }
            for (int i = (int)level + 1; i < minSkills.Length; ++i)
            {
                if (minSkills[i] < value)
                    minSkills[i] = value;
                else
                    break;
            }
        }

        public override void ExposeData()
		{
			base.ExposeData();
            float poor = GetSkillValue(SkillLevel.Poor);
            float normal = GetSkillValue(SkillLevel.Normal);
            float good = GetSkillValue(SkillLevel.Good);
            float excellent = GetSkillValue(SkillLevel.Excellent);
            float masterwork = GetSkillValue(SkillLevel.Masterwork);
            float legendary = GetSkillValue(SkillLevel.Legendary);

            Scribe_Values.Look(ref poor, "minskillpoor", 9);
			Scribe_Values.Look(ref normal, "minskillnormal", 13);
			Scribe_Values.Look(ref good, "minskillgood", 17);
			Scribe_Values.Look(ref excellent, "minskillexcellent", 21);
			Scribe_Values.Look(ref masterwork, "minskillmasterwork", 22);
			Scribe_Values.Look(ref legendary, "minskilllegendary", 19);
			Scribe_Values.Look(ref legendaryChance, "legendarychance", 5);
			Scribe_Values.Look(ref supressMasterworkMessages, "supressmasterworkmessages", false);
			Scribe_Values.Look(ref supressLegendaryMessages, "supresslegendarymessages", false);

            minSkills = new float[] { poor, normal, good, excellent, masterwork, legendary };
		}

        public void DoWindowContents(Rect inRect)
        {
            var list = new Listing_Standard();
            Color defaultColor = GUI.color;
            list.Begin(inRect);

            list.Label("CraftingQualityRebalanced.SliderWarning".Translate());

            float value = GetSkillValue(SkillLevel.Poor);
            list.Label("CraftingQualityRebalanced.MinimumSkillPoor".Translate() + (int)value);
            SetSkillValue(SkillLevel.Poor, (int)list.Slider(value, 0, 20));

            value = GetSkillValue(SkillLevel.Normal);
            list.Label("CraftingQualityRebalanced.MinimumSkillNormal".Translate() + (int)value);
            SetSkillValue(SkillLevel.Normal, (int)list.Slider(value, 0, 20));

            value = GetSkillValue(SkillLevel.Good);
            list.Label("CraftingQualityRebalanced.MinimumSkillGood".Translate() + (int)value);
            SetSkillValue(SkillLevel.Good, (int)list.Slider(value, 0, 20));

            value = GetSkillValue(SkillLevel.Excellent);
            list.Label("CraftingQualityRebalanced.MinimumSkillExcellent".Translate() + (int)value);
            SetSkillValue(SkillLevel.Excellent, (int)list.Slider(value, 0, 20));

            value = GetSkillValue(SkillLevel.Masterwork);
            list.Label("CraftingQualityRebalanced.MinimumSkillMasterwork".Translate() + (int)value);
            SetSkillValue(SkillLevel.Masterwork, (int)list.Slider(value, 0, 21));

            value = GetSkillValue(SkillLevel.Legendary);
            list.Label("CraftingQualityRebalanced.MinimumSkillLegendary".Translate() + (int)value);
            SetSkillValue(SkillLevel.Legendary, (int)list.Slider(value, 0, 21));

            list.Label("CraftingQualityRebalanced.LegendaryChance".Translate() + (int)legendaryChance + "%");
            legendaryChance = list.Slider(legendaryChance, 0, 100);

            list.Label("CraftingQualityRebalanced.LegendaryChanceExplanation".Translate());

            list.CheckboxLabeledSelectable("CraftingQualityRebalanced.SupressMasterworkMessages".Translate(), ref supressMasterworkMessages, ref supressMasterworkMessages);

            list.CheckboxLabeledSelectable("CraftingQualityRebalanced.SupressLegendaryMessages".Translate(), ref supressLegendaryMessages, ref supressLegendaryMessages);

            list.End();
        }

        private void BoundValues(ref float low, ref float value, ref float high)
        {
            if (low > value)
                low = value;
            if (value > high)
                high = value;
        }
    }
}
