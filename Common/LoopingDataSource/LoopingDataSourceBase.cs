using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace Common.LoopingDataSource
{
	public abstract class LoopingDataSourceBase : ILoopingSelectorDataSource
	{
		private object _selectedItem;

		public abstract object GetNext(object relativeTo);
		public abstract object GetPrevious(object relativeTo);
		public object SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				if (!Equals(_selectedItem, value))
				{
					object previousSelectedItem = _selectedItem;
					_selectedItem = value;
					
					OnSelectionChanged(previousSelectedItem, _selectedItem);
				}
			}
		}

		public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

		protected virtual void OnSelectionChanged(object oldSelectedItem, object newSelectedItem)
		{
			EventHandler<SelectionChangedEventArgs> handler = SelectionChanged;
			if (handler != null)
			{
				handler(this, new SelectionChangedEventArgs(new[] { oldSelectedItem }, new[] { newSelectedItem }));
			}
		}
	}
}
