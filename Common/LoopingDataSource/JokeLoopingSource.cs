using System.Collections.Generic;
using System.Linq;
using Common.Entities;

namespace Common.LoopingDataSource
{
	public class JokeLoopingSource : LoopingDataSourceBase
	{
		private readonly IList<JokeEntity> _objects;

		public JokeLoopingSource(IList<JokeEntity> objects)
		{
			_objects = objects;
			SelectedItem = _objects.FirstOrDefault();
		}

		public void SetSelectedItem(int index)
		{
			SelectedItem = _objects[index];
		}

		public override object GetNext(object relativeTo)
		{
			JokeEntity item = relativeTo as JokeEntity;
			if (item == null || item.Index >= _objects.Count-1)
			{
				return _objects[0];
			}
			
            return _objects[item.Index + 1];
		}

		public override object GetPrevious(object relativeTo)
		{
			JokeEntity item = relativeTo as JokeEntity;

			if (item == null || item.Index == 0)
			{
				return _objects.LastOrDefault();
			}

			return _objects[item.Index - 1];
		}
	}	

}
