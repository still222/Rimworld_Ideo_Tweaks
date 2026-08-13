using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace StkIdeoNerf;

[HarmonyPatch(typeof(ExpectationsUtility), nameof(ExpectationsUtility.CurrentExpectationFor), typeof(Pawn))]
public static class Patch_ExpectationsUtility_CurrentExpectationFor
{
	static void Postfix(Pawn p, ref ExpectationDef __result)
	{
		if (__result == null || !ModsConfig.IdeologyActive || __result == ExpectationDefOf.ExtremelyLow)
			return;

		Gender? supremeGender = p.Ideo?.SupremeGender;
		if (supremeGender == null || supremeGender == Gender.None || p.gender == supremeGender)
			return;
		
		__result = ExpectationsUtility.ExpectationForOrder(Mathf.Max(0, __result.order - 1), forRole: true);
	}

}
