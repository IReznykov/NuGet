using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Describe cellular automaton. 
	/// </summary>
	public class Automaton : IAutomaton
	{
		/// <summary>
		/// Private object that contains the set of cells.
		/// </summary>
		private CellSet CellSet { get; }

		/// <summary>
		/// Service that defines life flow.
		/// </summary>
		private ICellLifeService CellLifeService { get; }

		private readonly ILogger _logger;

		#region Init zone

		public Automaton(Size size, ICellLifeService cellLifeService, ILogger logger)
		{
			size.ThrowIfNull(nameof(size));
			if (size.Width < 5)
			{
				//throw new ArgumentOutOfRangeException(nameof(Size), size.Width, "Width of automaton should be more than 3");
				logger.Log($"{nameof(size.Width)} of automaton should be more than 5");
			}
			if (size.Height < 5)
			{
				//throw new ArgumentOutOfRangeException(nameof(Size), size.Height, "Height of automaton should be more than 3");
				logger.Log($"{nameof(size.Height)} of automaton should be more than 5");
			}

			cellLifeService.ThrowIfNull(nameof(cellLifeService));
			CellLifeService = cellLifeService;

			logger.ThrowIfNull(nameof(logger));
			_logger = logger;

			Size = new Size(System.Math.Max(size.Width, 5), System.Math.Max(size.Height, 5));
			CellSet = new CellSet();
			Init();

		}

		public Automaton(int width, int height, ICellLifeService cellLifeService, ILogger logger)
			: this(new Size(width, height), cellLifeService, logger)
		{
		}

		/// <summary>
		/// Clear all cells.
		/// </summary>
		public void Clear()
		{
			for (var x = 0; x < Size.Width; x++)
				for (var y = 0; y < Size.Height; y++)
				{
					CellSet[x, y].Clear();
				}
			Count = 0;
			AgeStatistics.Init(Size.Height * Size.Width);
		}

		/// <summary>
		/// Initialization of inner fields and properties.
		/// </summary>
		protected void Init()
		{
			CellSet.Init(Size);
			Count = 0;
			AgeStatistics.Init(Size.Height * Size.Width);
		}

		/// <summary>
		/// Method sets initial cells to automaton.
		/// </summary>
		/// <param name="newPoints">Set of new cells that are added to automaton.</param>
		/// <param name="statistics">Statistics with count of changed cells</param>
		public void SetPoints(IEnumerable<Point> newPoints, ref Statistics statistics)
		{
			if (newPoints == null)
				return;

			foreach (var point in newPoints.Where(point => !CellSet[point.X, point.Y].State))
			{ 
				CellSet[point.X, point.Y].NextState = true;
			}
			ApplyChanges(ref statistics);
		}

		/// <summary>
		/// Method update current cells.
		/// </summary>
		/// <param name="addedPoints">Set of new cells that are added to automaton.</param>
		/// <param name="removedPoints">Set of cells that are killed in automaton.</param>
		/// <param name="statistics">Statistics with count of changed cells</param>
		public void UpdatePoints(IEnumerable<Point> addedPoints, IEnumerable<Point> removedPoints, ref Statistics statistics)
		{
			if (addedPoints == null)
				return;

			foreach (var point in addedPoints.Where(point => !CellSet[point.X, point.Y].State))
			{
				CellSet[point.X, point.Y].NextState = true;
			}

			foreach (var point in removedPoints.Where(point => CellSet[point.X, point.Y].State))
			{
				CellSet[point.X, point.Y].NextState = false;
			}

			ApplyChanges(ref statistics);
		}


		/// <summary>
		/// Method returns current automaton points.
		/// </summary>
		public IEnumerable<Point> GetPoints()
		{
			var points = new List<Point>();

			for (var x = 0; x < Size.Width; x++)
				for (var y = 0; y < Size.Height; y++)
				{
					if (CellSet[x, y].State)
						points.Add(new Point(x, y));
				}

			return points;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Size of cellular automaton.
		/// </summary>
		public Size Size { get; }

		/// <summary>
		/// Return count of living cells.
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// Series of cells count depending on their age.
		/// </summary>
		public IEnumerable<int> AgeSeries => AgeStatistics.GetSeries();

		/// <summary>
		/// Inner object that keep series of cells count depending on their age.
		/// </summary>
		private AgeStatistics AgeStatistics { get; } = new AgeStatistics();

		#endregion

		#region Iterations

		/// <summary>
		/// Lock object is used to reject iteration calls
		/// until current one will complete.
		/// </summary>
		private readonly object _lockObject = new object();

		/// <summary>
		/// Moves cells to the next stage - depends on used model
		/// they will survive, die, or born.
		/// </summary>
		/// <param name="statistics">Statistics with count of changed cells</param>
		public bool Iterate(ref Statistics statistics)
		{
			var acquiredLock = false;
			try
			{
				// check the possibility to lock
				Monitor.TryEnter(_lockObject, ref acquiredLock);
				if (!acquiredLock)
				{
					_logger.Log("Iteration is still processing, pass to next call.");
					return false;
				}

				// step 1. Calculates next states of the cells
				for (var y = 0; y < Size.Height; y++)
					for (var x = 0; x < Size.Width; x++)
					{
						CellLifeService.Iterate(CellSet[x - 1, y], CellSet[x, y], CellSet[x + 1, y]);
					}

				// step 2. Apply changes and clear next state
				ApplyChanges(ref statistics);
				return true;
			}
			finally
			{
				if (acquiredLock)
				{
					// release monitor if lock was set
					Monitor.Exit(_lockObject);
				}
			}
		}

		private void ApplyChanges(ref Statistics statistics)
		{
			lock (_lockObject)
			{
				statistics.Clear();
				// Apply changes and clear next state
				for (var y = 0; y < Size.Height; y++)
					for (var x = 0; x < Size.Width; x++)
					{
						var delta = CellSet[x, y].Delta;
						var age = CellSet[x, y].Age;

						if (delta != 0)
						{
							CellSet[x, y - 1].AddVertSum(delta);
							CellSet[x, y + 1].AddVertSum(delta);
							Count += delta;
							if (delta > 0)
							{
								++statistics.Borned;
								AgeStatistics.Borned();
							}
							else // delta < 0
							{
								--statistics.Died;
								AgeStatistics.Died(age);
							}
						}
						else if (CellSet[x, y].State)
						{
							// cell was survive
							AgeStatistics.Survive(age);
						}
						// should apply to all cells - it increments age
						CellSet[x, y].ApplyChange();
					}
			}
		}

		/// <summary>
		/// Returns (reference to) cell object at specified coordinates.
		/// </summary>
		/// <param name="x">Row.</param>
		/// <param name="y">Column.</param>
		/// <returns></returns>
		public ICell GetCell(int x, int y)
		{
			return CellSet[x, y];
		}

		#endregion

	}
}
