using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace StkIdeoNerf;

[HarmonyPatch(typeof(ExpectationsUtility), nameof(ExpectationsUtility.CurrentExpectationFor), typeof(Pawn))]
public static class Patch_ExpectationsUtility_CurrentExpectationFor
{
	[HarmonyPostfix]
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

[HarmonyPatch(typeof(ThoughtWorker_ExpectationsSlave), nameof(ThoughtWorker_ExpectationsSlave.CurrentStateInternal))]
public static class Patch_ExpectationsSlave_CurrentStateInternal
{
	private static int GetThoughtStageForExpectationOppressed(ExpectationDef expectation)
	{
		if (expectation == ExpectationDefOf.ExtremelyLow)
			return 0;
		if (expectation == ExpectationDefOf.VeryLow)
			return 1;

		return 3;
	}

	// Transplier

}
