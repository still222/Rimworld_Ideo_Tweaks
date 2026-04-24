using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StkIdeoNerf;

[HarmonyPatch(typeof(ExpectationsUtility), nameof(ExpectationsUtility.CurrentExpectationFor), typeof(Pawn))]
public static class Patch_ExpectationsUtility_CurrentExpectationFor
{
	static void Postfix(Pawn p, ref ExpectationDef __result)
	{
		if (__result == null || !ModsConfig.IdeologyActive)
			return;

		var ideol = p?.Ideo;
		if (ideol?.memes == null)
			return;

		bool isFemaleSupremacy = ideol.HasMeme(MemeDefOf.FemaleSupremacy);
		bool isMaleSupremacy = ideol.HasMeme(MemeDefOf.MaleSupremacy);
		if (!isFemaleSupremacy && !isMaleSupremacy)
			return;

		bool isOppressed =
			(isFemaleSupremacy && p.gender == Gender.Male) ||
			(isMaleSupremacy && p.gender == Gender.Female);
		if (!isOppressed)
			return;

		int newOrder = Math.Max(0, __result.order - 1);
		
		var lowered = ExpectationsUtility.ExpectationForOrder(newOrder, forRole: true);
		if (lowered != null)
			__result = lowered;
	}
}