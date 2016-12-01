using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Ikc5.TypeLibrary;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Class implements the cell of cellular automaton. Properties are thread-safe.
	/// It inherits BindableBase class in order to notify about properties changes.
	/// </summary>
	public class Cell : BaseNotifyPropertyChanged, ICell
	{
		/// <summary>
		/// Properties are thread-safe, so setters use this object for locking. 
		/// </summary>
		private readonly object _lockObject = new object();

		private bool _state;
		private short _vertSum;
		private short _age;
		private bool? _nextState;

		/// <summary>
		/// Current state of the cell - Living/Dead.
		/// </summary>
		[DefaultValue(false)]
		public bool State
		{
			get
			{
				lock (_lockObject)
				{
					return _state;
				}
			}
			protected internal set
			{
				// as VertSum includes state of the current cell, its value is
				// changed depends whether cell wil born or die
				lock (_lockObject)
				{
					if (_state == value)
						return;

					var delta = _state ? -1 : 1;
					SetProperty(ref _state, value);
					VertSum += (short)delta;
					if (!_state)
						Age = 0;    // cell dies
					else if (Age == 0)
						Age = 1;
				}
			}
		}

		/// <summary>
		/// Count of live cells among the current one and two nearest neighbors
		/// - above and below.
		/// </summary>
		[DefaultValue(0)]
		public short VertSum
		{
			get
			{
				lock (_lockObject)
				{
					return _vertSum;
				}
			}
			protected internal set
			{
				lock (_lockObject)
				{
					if (value < 0)
						throw new ArgumentException("Vertical count of cells should be non-negative", nameof(VertSum));
					SetProperty(ref _vertSum, value);
				}
			}
		}

		/// <summary>
		/// Update count of living cells around current one.
		/// </summary>
		public void AddVertSum(short delta)
		{
			lock (_lockObject)
			{
				VertSum += delta;
			}
		}

		public short Age
		{
			get
			{
				lock (_lockObject)
				{
					return _age;
				}
			}
			private set
			{
				lock (_lockObject)
				{
					SetProperty(ref _age, value);
				}
			}
		}

		/// <summary>
		/// Next state of the cell, used as buffered value during iteration.
		/// </summary>
		[DefaultValue(null)]
		public bool? NextState
		{
			get
			{
				lock (_lockObject)
				{
					return _nextState;
				}
			}
			set
			{
				lock (_lockObject)
				{
					SetProperty(ref _nextState, value);
				}
			}
		}

		/// <summary>
		/// TRUE if next state and current state are different, otherwise are equal.
		/// </summary>
		public bool IsChanged
		{
			get
			{
				lock (_lockObject)
				{
					return _nextState.HasValue && (_state != _nextState.Value);
				}
			}
		}

		/// <summary>
		/// Integer equivalence of the difference betwee next state and current state:
		/// 0 - if they are the same;
		/// 1 - if cell will born;
		/// -1 - if cell wil die.
		/// </summary>
		public short Delta
		{
			get
			{
				lock (_lockObject)
				{
					if (_nextState == null)
						return 0;
					return (short)((_nextState.Value ? 1 : 0) - (_state ? 1 : 0));
				}
			}
		}

		/// <summary>
		/// Cell goes to next state.
		/// </summary>
		public void ApplyChange()
		{
			lock (_lockObject)
			{
				if (IsChanged)
				{
					State = NextState.Value;
				}
				else if (State)
					++Age;

				NextState = null;
			}
		}

		/// <summary>
		/// Method initiates a cell by default values.
		/// </summary>
		public void Clear()
		{
			this.SetDefaultValue(nameof(State));
			this.SetDefaultValue<bool?>(nameof(NextState));
			this.SetDefaultValue<short>(nameof(VertSum));
		}
	}
}
