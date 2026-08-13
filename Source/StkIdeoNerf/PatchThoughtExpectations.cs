using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StkIdeoNerf;

[HarmonyPatch]
public static class PatchThoughtExpectations
{
	static IEnumerable<MethodBase> TargetMethods()
	{
		yield return AccessTools.Method(typeof(Pawn_AgeTracker), nameof(Pawn_AgeTracker.AgeTickInterval));
		yield return AccessTools.Method(typeof(ThoughtUtility), nameof(ThoughtUtility.CanGetThought));
	}

	static readonly MethodInfo mapExpectation =
	AccessTools.Method(
		typeof(ExpectationsUtility),
		nameof(ExpectationsUtility.CurrentExpectationFor),
		[typeof(Map)]
	);
	static readonly MethodInfo pawnExpectation =
	AccessTools.Method(
		typeof(ExpectationsUtility),
		nameof(ExpectationsUtility.CurrentExpectationFor),
		[typeof(Pawn)]
	);

	[HarmonyTranspiler]
	public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		if (!StkIdeoNerfSettings.fixThoughtExpectations)
			return instructions;

		var codes = new List<CodeInstruction>(instructions);
		for (int i = 1; i < codes.Count; i++)
		{
			if (!codes[i].Calls(mapExpectation))
				continue;

			codes[i].operand = pawnExpectation;
			codes.RemoveAt(i - 1);		// Removes .MapHeld call
			break;
		}

		return codes;
	}

}
