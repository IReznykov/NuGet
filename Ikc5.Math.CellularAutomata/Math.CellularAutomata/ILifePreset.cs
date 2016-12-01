namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Contains parameters for born/survive decisions.
	/// </summary>
	public interface ILifePreset
	{
		/// <summary>
		/// Returns will cell born if it has exact neighbors count.
		/// </summary>
		/// <param name="neighborCount">Neighbors count</param>
		/// <returns></returns>
		bool Born(int neighborCount);

		/// <summary>
		/// Returns will cell survive if it has exact neighbors count.
		/// </summary>
		/// <param name="neighborCount">Neighbors count</param>
		/// <returns></returns>
		bool Survive(int neighborCount);
	}
}