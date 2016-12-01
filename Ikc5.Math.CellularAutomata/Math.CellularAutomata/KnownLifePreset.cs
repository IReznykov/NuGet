using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// https://en.wikipedia.org/wiki/Life-like_cellular_automaton
	/// https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life
	/// </summary>
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum KnownLifePreset : byte
	{
		/// <summary>
		/// Conway's Game of Life, highly complex behavior.
		/// </summary>
		[Description("Conway's Game of Life (B3/S23)")]
		Life = 0,           // B3/S23

		/// <summary>
		/// Replicator Edward Fredkin's replicating automaton: every pattern is
		/// eventually replaced by multiple copies of itself.
		/// </summary>
		[Description("Edward Fredkin's replicaton (B1357/S1357)")]
		Replicator,         // B1357/S1357

		/// <summary>
		/// All patterns are phoenixes, meaning that every live cell
		/// immediately dies, and many patterns lead to explosive chaotic growth.
		/// However, some engineered patterns with complex behavior are known.
		/// </summary>
		[Description("Seeds (B2/S)")]
		Seeds,				// B2/S

		/// <summary>
		/// This rule supports a small self-replicating pattern which, when combined
		/// with a small glider pattern, causes the glider to bounce back and forth
		/// in a pseudorandom walk.
		/// </summary>
		[Description("Pseudorandom (B25/S4)")]
		Pseudorandom,		// B25/S4

		/// <summary>
		/// Also known as Inkspot or Flakes. Cells that become alive never die. It combines
		/// chaotic growth with more structured ladder-like patterns that can be used to
		/// simulate arbitrary Boolean circuits.
		/// </summary>
		[Description("Life without death(B3/S012345678)")]
		LifeWithoutDeath,   // B3/S012345678

		/// <summary>
		/// Was initially thought to be a stable alternative to Life, until computer simulation
		/// found that larger patterns tend to explode. Has many small oscillators and spaceships.
		/// </summary>
		[Description("Life 34 (B34/S34)")]
		Life34,				// B34/S34

		/// <summary>
		/// Forms large diamonds with chaotically fluctuating boundaries. First studied by Dean Hickerson, who 
		/// in 1993 offered a $50 prize to find a pattern that fills space with live cells;
		/// the prize was won in 1999 by David Bell.
		/// </summary>
		[Description("Diamoeba (B35678/S5678)")]
		Diamoeba,			// B35678/S5678

		/// <summary>
		/// If a pattern is composed of 2x2 blocks, it will continue to evolve in the same form; 
		/// grouping these blocks into larger powers of two leads to the same behavior, but slower. 
		/// Has complex oscillators of high periods as well as a small glider.
		/// </summary>
		[Description("Pattern 2x2 (B36/S125)")]
		Pattern2x2,			// B36/S125

		/// <summary>
		/// Similar to Life but with a small self-replicating pattern.
		/// </summary>
		[Description("High life (B36/S23)")]
		HighLife,           // B36/S23

		/// <summary>
		/// Symmetric under on-off reversal. Has engineered patterns with highly complex behavior.
		/// </summary>
		[Description("Day and night (B3678/S34678)")]
		DayNight,			// B3678/S34678

		/// <summary>
		/// Named after Stephen Morley; also called Move. Supports very high-period and slow spaceships.
		/// </summary>
		[Description("Stephen Morley's Move (B368/S245)")]
		Morley,             // B368/S245

		/// <summary>
		/// Also called the twisted majority rule. Symmetric under on-off reversal. Approximates the
		/// curve-shortening flow on the boundaries between live and dead cells.
		/// </summary>
		[Description("Twisted majority rule (B4678/S35678)")]
		Anneal,				// B4678/S35678 

	}
}
