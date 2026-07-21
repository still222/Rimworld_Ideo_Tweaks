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

		var ideo = p.Ideo;
		if (ideo?.memes == null)
			return;

		bool isFemaleSupremacy = ideo.HasMeme(MemeDefOf.FemaleSupremacy);
		bool isMaleSupremacy = ideo.HasMeme(MemeDefOf.MaleSupremacy);
		if (!isFemaleSupremacy && !isMaleSupremacy)
			return;

		bool isOppressed =
			(isFemaleSupremacy && p.gender == Gender.Male) ||
			(isMaleSupremacy && p.gender == Gender.Female);
		if (!isOppressed)
			return;
		
		var lowered = ExpectationsUtility.ExpectationForOrder(
			Math.Max(0, __result.order - 1),
			forRole: true
		);

		__result = lowered;
	}

}
