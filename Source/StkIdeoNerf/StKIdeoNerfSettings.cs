using UnityEngine;
using Verse;

namespace StkIdeoNerf;
public class StkIdeoNerfSettings : ModSettings
{
	public static bool fixThoughtExpectations = false;

	public override void ExposeData()
	{
		base.ExposeData();

		Scribe_Values.Look(ref fixThoughtExpectations, "StkIdeoNerf.fixThoughtExpectations", false);
	}
}

public class StkIdeoNerfMod : Mod
{
	public StkIdeoNerfMod(ModContentPack content) : base(content)
	{
		GetSettings<StkIdeoNerfSettings>();
	}

	public override string SettingsCategory() => "StK Ideo Tweaks";
	public override void DoSettingsWindowContents(Rect canvas)
	{
		Listing_Standard listing = new();
		listing.Begin(canvas);
		listing.Gap();

		listing.CheckboxLabeled("stkfixThoughtExpectations".TranslateSimple(),
			ref StkIdeoNerfSettings.fixThoughtExpectations);
		listing.End();
	}

}

