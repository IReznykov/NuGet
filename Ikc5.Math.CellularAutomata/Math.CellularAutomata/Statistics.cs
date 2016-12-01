namespace Ikc5.Math.CellularAutomata
{
	public struct Statistics
	{
		public Statistics(int borned, int died)
			: this()
		{
			Borned = borned;
			Died = died;
		}

		/// <summary>
		/// Count of new born cells.
		/// </summary>
		public int Borned { get; set; }

		/// <summary>
		/// Count of new born cells.
		/// </summary>
		public int Died { get; set; }

		/// <summary>
		/// Count of changed cells.
		/// </summary>
		public int Changed => Borned + System.Math.Abs(Died);

		/// <summary>
		/// Total count of all living cell in automaton.
		/// </summary>
		//public int Total { get; set; }

		public void Clear()
		{
			Borned = 0;
			Died = 0;
		}
	}
}

