using System;

namespace Ikc5.Math.CellularAutomata
{
	public static class KnownLifePresets
	{
		#region Predefined objects

		/// <summary>
		/// Conway's Game of Life, highly complex behavior.
		/// </summary>
		public static ILifePreset Life => new LifePreset(new[] { 3 }, new[] { 2, 3 });

		/// <summary>
		/// Replicator Edward Fredkin's replicating automaton: every pattern is
		/// eventually replaced by multiple copies of itself.
		/// </summary>
		public static ILifePreset Replicator => new LifePreset(new[] { 1, 3, 5, 7 }, new[] { 1, 3, 5, 7 });

		/// <summary>
		/// All patterns are phoenixes, meaning that every live cell
		/// immediately dies, and many patterns lead to explosive chaotic growth.
		/// However, some engineered patterns with complex behavior are known.
		/// </summary>
		public static ILifePreset Seeds => new LifePreset(new[] { 2 }, new int[0]);

		/// <summary>
		/// This rule supports a small self-replicating pattern which, when combined
		/// with a small glider pattern, causes the glider to bounce back and forth
		/// in a pseudorandom walk.
		/// </summary>
		public static ILifePreset Pseudorandom => new LifePreset(new[] { 2, 5 }, new[] { 4 });

		/// <summary>
		/// Also known as Inkspot or Flakes. Cells that become alive never die. It combines
		/// chaotic growth with more structured ladder-like patterns that can be used to
		/// simulate arbitrary Boolean circuits.
		/// </summary>
		public static ILifePreset LifeWithoutDeath => new LifePreset(new[] { 3 }, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

		/// <summary>
		/// Was initially thought to be a stable alternative to Life, until computer simulation
		/// found that larger patterns tend to explode. Has many small oscillators and spaceships.
		/// </summary>
		public static ILifePreset Life34 => new LifePreset(new[] { 3, 4 }, new[] { 3, 4 });

		/// <summary>
		/// Forms large diamonds with chaotically fluctuating boundaries. First studied by Dean Hickerson, who 
		/// in 1993 offered a $50 prize to find a pattern that fills space with live cells;
		/// the prize was won in 1999 by David Bell.
		/// </summary>
		public static ILifePreset Diamoeba => new LifePreset(new[] { 3, 5, 6, 7, 8 }, new[] { 5, 6, 7, 8 });

		/// <summary>
		/// If a pattern is composed of 2x2 blocks, it will continue to evolve in the same form; 
		/// grouping these blocks into larger powers of two leads to the same behavior, but slower. 
		/// Has complex oscillators of high periods as well as a small glider.
		/// </summary>
		public static ILifePreset Pattern2x2 => new LifePreset(new[] { 3, 6 }, new[] { 1, 2, 5 });

		/// <summary>
		/// Similar to Life but with a small self-replicating pattern.
		/// </summary>
		public static ILifePreset HighLife => new LifePreset(new[] { 3, 6 }, new[] { 2, 3 });

		/// <summary>
		/// Symmetric under on-off reversal. Has engineered patterns with highly complex behavior.
		/// </summary>
		public static ILifePreset DayNight => new LifePreset(new[] { 3, 6, 7, 8 }, new[] { 3, 4, 6, 7, 8 });

		/// <summary>
		/// Named after Stephen Morley; also called Move. Supports very high-period and slow spaceships.
		/// </summary>
		public static ILifePreset Morley => new LifePreset(new[] { 3, 6, 8 }, new[] { 2, 4, 5 });

		/// <summary>
		/// Also called the twisted majority rule. Symmetric under on-off reversal. Approximates the
		/// curve-shortening flow on the boundaries between live and dead cells.
		/// </summary>
		public static ILifePreset Anneal => new LifePreset(new[] { 4, 6, 7, 8 }, new[] { 3, 5, 6, 7, 8 });

		#endregion Predefined objects

		public static ILifePreset GetKnownLifePreset(KnownLifePreset lifePreset)
		{
			switch (lifePreset)
			{
			case KnownLifePreset.Life:
				return Life;
			case KnownLifePreset.Replicator:
				return Replicator;
			case KnownLifePreset.Seeds:
				return Seeds;
			case KnownLifePreset.Pseudorandom:
				return Pseudorandom;
			case KnownLifePreset.LifeWithoutDeath:
				return LifeWithoutDeath;
			case KnownLifePreset.Life34:
				return Life34;
			case KnownLifePreset.Diamoeba:
				return Diamoeba;
			case KnownLifePreset.Pattern2x2:
				return Pattern2x2;
			case KnownLifePreset.HighLife:
				return HighLife;
			case KnownLifePreset.DayNight:
				return DayNight;
			case KnownLifePreset.Morley:
				return Morley;
			case KnownLifePreset.Anneal:
				return Anneal;
			default:
				throw new ArgumentOutOfRangeException(nameof(lifePreset));
			}
		}
	}
}
